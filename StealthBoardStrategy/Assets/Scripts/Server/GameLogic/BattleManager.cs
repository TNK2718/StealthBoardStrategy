using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using StealthBoardStrategy.Frontend.Client;
using StealthBoardStrategy.Frontend.Events;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;

namespace StealthBoardStrategy.Server.GameLogic {
    [RequireComponent (typeof (PhotonView))]
    public class BattleManager : MonoBehaviourPunCallbacks {
        const float SELECTING_TIME = 60;
        private int TurnProcessed;
        private float RemainingTime = SELECTING_TIME;
        private Players Turn;
        private GameState GameState = GameState.None;
        private List<Unit> UnitList1;
        private List<Unit> UnitList2;
        private Board Board;

        public GameObject MasterPlayer;
        public GameObject GuestPlayer;

        private void Start () {
            // ここからはMasterClientのみ
            if (!PhotonNetwork.IsMasterClient) return;
            Board = new Board ();
            // プレイヤーユニットを生成
            UnitList1 = new List<Unit> { new Unit (0, Players.Player1, 0, 0, Players.Player1) };
            UnitList2 = new List<Unit> { new Unit (0, Players.Player2, 0, 0, Players.Player2) };
            GameState = GameState.Matching;
            Turn = Players.Player1;
        }

        private void FixedUpdate () {
            //if (PhotonNetwork.IsMasterClient) Debug.Log (GameState);
            if (GameState == GameState.WaitingForInput) {
                RemainingTime -= Time.fixedDeltaTime;
                if (RemainingTime <= 0) {
                    EndPhase ();
                }
            } else if (GameState == GameState.Matching) {
                if (MasterPlayer != null && GuestPlayer != null) {
                    PrePhase ();
                }
            }
        }

        // クライアントからマスターへの送信
        [PunRPC]
        public void SendEventToMaster (string msg, string gameEventJson) {
            GameEvent gameEvent;
            if (!PhotonNetwork.IsMasterClient) return;
            if (msg == "ActionEvent") {
                gameEvent = JsonUtility.FromJson<ActionEvent>(gameEventJson);
                ActionPhase ((ActionEvent) gameEvent);
            } else if (msg == "ReadyEvent") {
                RespondToReadyEvent ();
            } else {

            }
        }

        // クライアント側にボードを送信
        public void SyncBoardToClients () {
            if (!PhotonNetwork.IsMasterClient) return;
            try {
                string boardJson = JsonUtility.ToJson (Board);
                ClientUnit[] unitList11 = new ClientUnit[UnitList1.Count];
                ClientUnit[] unitList12 = new ClientUnit[UnitList1.Count];
                ClientUnit[] unitList21 = new ClientUnit[UnitList2.Count];
                ClientUnit[] unitList22 = new ClientUnit[UnitList2.Count];
                for (int i = 0; i < UnitList1.Count; i++) {
                    unitList11[i] = UnitList1[i].ConvertToClientUnit (false);
                    unitList12[i] = UnitList1[i].ConvertToClientUnit (true);
                }
                for (int i = 0; i < UnitList2.Count; i++) {
                    unitList21[i] = UnitList2[i].ConvertToClientUnit (true);
                    unitList22[i] = UnitList2[i].ConvertToClientUnit (false);
                }
                string[] unitListJson11 = new string[unitList11.Length];
                string[] unitListJson12 = new string[unitList12.Length];
                string[] unitListJson21 = new string[unitList21.Length];
                string[] unitListJson22 = new string[unitList22.Length];
                for (int i = 0; i < unitList11.Length; i++) {
                    unitListJson11[i] = JsonUtility.ToJson (unitList11[i]);
                }
                for (int i = 0; i < unitList12.Length; i++) {
                    unitListJson12[i] = JsonUtility.ToJson (unitList12[i]);
                }
                for (int i = 0; i < unitList21.Length; i++) {
                    unitListJson21[i] = JsonUtility.ToJson (unitList21[i]);
                }
                for (int i = 0; i < unitList22.Length; i++) {
                    unitListJson22[i] = JsonUtility.ToJson (unitList22[i]);
                }
                object[] args1 = new object[] {
                    "SyncBoard",
                    boardJson,
                    unitListJson11,
                    unitListJson21
                };
                object[] args2 = new object[] {
                    "SyncBoard",
                    boardJson,
                    unitListJson12,
                    unitListJson22
                };

                MasterPlayer.GetComponent<PhotonView> ().RPC ("SyncBoard", RpcTarget.AllViaServer, args1);
                GuestPlayer.GetComponent<PhotonView> ().RPC ("SyncBoard", RpcTarget.AllViaServer, args2);
            } catch {

            }
        }

        // ターン開始前の処理
        private void PrePhase () {
            if (!PhotonNetwork.IsMasterClient) return;
            GameState = GameState.TurnStart;
            // 処理
            // イベント送信&同期
            object[] args1 = new object[] { "TrunStartEventToClient", JsonUtility.ToJson(new TurnStartEventToClient ()) };
            object[] args2 = new object[] { "TrunStartEventToClient", JsonUtility.ToJson(new TurnStartEventToClient ()) };
            MasterPlayer.GetComponent<PhotonView> ().RPC ("RecieveEvent", RpcTarget.AllViaServer, args1);
            GuestPlayer.GetComponent<PhotonView> ().RPC ("RecieveEvent", RpcTarget.AllViaServer, args2);
            SyncBoardToClients ();
        }
        // ターン開始
        private void TurnStart () {
            if (!PhotonNetwork.IsMasterClient) return;
            RemainingTime = SELECTING_TIME;

            // 入力を受付
            GameState = GameState.WaitingForInput;
        }
        // クライアントから送られてきたActionEventを受け取って処理
        // TODO: プレイヤーの識別, 認証
        private void ActionPhase (ActionEvent actionEvent) {
            if (!PhotonNetwork.IsMasterClient) return;
            if (!(GameState == GameState.WaitingForInput)) return;
            if (actionEvent.Sender != Turn) return;
            if (GetUnitList (actionEvent.Sender) [actionEvent.Invoker].ActionPoint <= 0) return;

            GameState = GameState.AccepetedInput;
            if (actionEvent.ActionNo == 0) {
                // 移動
                Move (GetUnitList (actionEvent.Sender) [actionEvent.Invoker], (actionEvent.TargetPositionX, actionEvent.TargetPositionY));
            } else {
                // 番号に対応するスキルを発動
                try {
                    switch (GetUnitList (actionEvent.Sender) [actionEvent.Invoker].SkillList[actionEvent.ActionNo].SkillType) {
                        case SkillType.Attack:
                            break;
                        default:
                            Debug.Log ("No skill corresponds");
                            break;
                    }
                } catch {
                    Debug.LogAssertion ("IndexOutofRange");
                }
            }
            // クライアントと同期
            SyncBoardToClients ();
        }

        // ReadyEventに対する対応
        private void RespondToReadyEvent () {
            if (!PhotonNetwork.IsMasterClient) return;
            if (GameState == GameState.AccepetedInput) {
                GameState = GameState.WaitingForInput;
            } else if (GameState == GameState.TurnStart) {
                TurnStart ();
            } else if (GameState == GameState.TurnEnd) {
                // 次のターンへ移行
                TurnEnd ();
            } else {

            }
        }
        // ターン終了時の処理
        // dotダメージや建造物の効果などはここで発動する
        private void EndPhase () {
            if (!PhotonNetwork.IsMasterClient) return;
            GameState = GameState.TurnEnd;

            // イベント送信&同期
            object[] args1 = new object[] { "TrunEndEventToClient", JsonUtility.ToJson (new TurnEndEventToClient ()) };
            object[] args2 = new object[] { "TrunEndEventToClient", JsonUtility.ToJson (new TurnEndEventToClient ()) };
            MasterPlayer.GetComponent<PhotonView> ().RPC ("RecieveEvent", RpcTarget.AllViaServer, args1);
            GuestPlayer.GetComponent<PhotonView> ().RPC ("RecieveEvent", RpcTarget.AllViaServer, args2);
            SyncBoardToClients ();
        }
        // ターン終了
        private void TurnEnd () {
            if (!PhotonNetwork.IsMasterClient) return;
            if (Turn == Players.Player1) {
                Turn = Players.Player2;
            } else if (Turn == Players.Player2) {
                Turn = Players.Player1;
            }
            PrePhase ();
        }

        //
        // スキルの実装
        //
        private void Move (Unit invoker, (int x, int y) position) {
            // float squareddistanse;
            // if (invoker.GetSpeed () * invoker.GetSpeed () >= squareddistanse) {
            // } else {
            //     // 不正な入力を通知
            // }
        }
        private void Attack () {

        }

        private List<Unit> GetUnitList (Players player) {
            if (player == Players.Player1) {
                return UnitList1;
            } else if (player == Players.Player2) {
                return UnitList2;
            } else {
                return null;
            }
        }
    }
}