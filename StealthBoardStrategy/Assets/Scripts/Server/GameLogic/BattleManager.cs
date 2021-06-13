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
        private float RemainingTime = SELECTING_TIME;
        private GameState GameState;
        private BattleLogic BattleLogic;
        private ActionEvent actionEvent1;
        private ActionEvent actionEvent2;
        private bool Ready1;
        private bool Ready2;

        public GameObject MasterPlayer;
        public GameObject GuestPlayer;
        public SkillList SkillList;

        private void Start () {
            // ここからはMasterClientのみ
            if (!PhotonNetwork.IsMasterClient) return;
            GameState = GameState.Matching;
            BattleLogic = new BattleLogic ();
            SkillList = new SkillList ();
        }

        private void FixedUpdate () {
            SwitchGameState ();
        }

        // クライアントからマスターへの送信
        [PunRPC]
        public void SendEventToMaster (string msg, string gameEventJson) {
            GameEvent gameEvent;
            if (!PhotonNetwork.IsMasterClient) return;
            if (msg == "ActionEvent") {
                gameEvent = JsonUtility.FromJson<ActionEvent> (gameEventJson);
                RespondToActionEvent ((ActionEvent) gameEvent);
            } else if (msg == "ReadyEvent") {
                gameEvent = JsonUtility.FromJson<ReadyEvent> (gameEventJson);
                RespondToReadyEvent ((ReadyEvent) gameEvent);
            } else {

            }
        }

        // クライアント側にボードを送信
        public void SyncBoardToClients () {
            if (!PhotonNetwork.IsMasterClient) return;
            var argsarray = BattleLogic.SerializeBoard ();
            MasterPlayer.GetComponent<PhotonView> ().RPC ("SyncBoard", RpcTarget.AllViaServer, argsarray[0]);
            GuestPlayer.GetComponent<PhotonView> ().RPC ("SyncBoard", RpcTarget.AllViaServer, argsarray[1]);
        }

        // クライアントから送られてきたActionEventを受け取る
        // TODO: プレイヤーの識別, 認証
        private void RespondToActionEvent (ActionEvent actionEvent) {
            Debug.Log ("Recieved ActionEvent!");

            if (!PhotonNetwork.IsMasterClient) return;
            if (!(GameState == GameState.WaitingForInput)) return;

            if (actionEvent.Sender == Players.Player1) {
                actionEvent1 = actionEvent;
                // 入力完了フラグ
                Ready1 = true;
            } else if (actionEvent.Sender == Players.Player2) {
                actionEvent2 = actionEvent;
                // 入力完了フラグ
                Ready2 = true;
            } else {
                Debug.LogAssertion ("UnknownSender");
            }
        }

        // ReadyEventに対する対応
        private void RespondToReadyEvent (ReadyEvent readyEvent) {
            if (!PhotonNetwork.IsMasterClient) return;

            if (readyEvent.Sender == Players.Player1) {
                Ready1 = true;
            } else if (readyEvent.Sender == Players.Player2) {
                Ready2 = true;
            } else {
                Debug.LogAssertion ("UnknownSender");
            }
        }
        // ターン開始前の処理
        private void PrePhase () {
            if (!PhotonNetwork.IsMasterClient) return;
            GameState = GameState.TurnStart;
            // ActionEventsを初期化
            actionEvent1 = null;
            actionEvent2 = null;
            Ready1 = false;
            Ready2 = false;
            // 処理

            // イベント送信&同期
            object[] args1 = new object[] { "TrunStartEventToClient", JsonUtility.ToJson (new TurnStartEventToClient ()) };
            object[] args2 = new object[] { "TrunStartEventToClient", JsonUtility.ToJson (new TurnStartEventToClient ()) };
            MasterPlayer.GetComponent<PhotonView> ().RPC ("RecieveEvent", RpcTarget.AllViaServer, args1);
            GuestPlayer.GetComponent<PhotonView> ().RPC ("RecieveEvent", RpcTarget.AllViaServer, args2);
            SyncBoardToClients ();
        }

        // ターン開始
        private void TurnStart () {
            if (!PhotonNetwork.IsMasterClient) return;
            Debug.Log("TurnStart");

            RemainingTime = SELECTING_TIME;

            // 入力を受付
            GameState = GameState.WaitingForInput;
            Ready1 = false;
            Ready2 = false;
        }

        // 受け取った入力を処理+結果をクライアントに送信
        // dotダメージや建造物の効果などはEndPhaseの最後で発動する
        private void EndPhase () {
            if (!PhotonNetwork.IsMasterClient) return;
            Debug.Log ("EndPhase");

            GameState = GameState.TurnEnd;
            Ready1 = false;
            Ready2 = false;

            // TODO: 入力を元にactionを処理、イベント生成
            ActionEventToClient actionEventToClient = BattleLogic.ProcessActionEvents (actionEvent1, actionEvent2);

            // イベント送信&同期
            object[] args1 = new object[] { "ActionEventToClient", JsonUtility.ToJson (actionEventToClient) };
            object[] args2 = new object[] { "ActionEventToClient", JsonUtility.ToJson (actionEventToClient) };
            MasterPlayer.GetComponent<PhotonView> ().RPC ("RecieveEvent", RpcTarget.AllViaServer, args1);
            GuestPlayer.GetComponent<PhotonView> ().RPC ("RecieveEvent", RpcTarget.AllViaServer, args2);
            SyncBoardToClients ();
        }

        // ターン終了
        private void TurnEnd () {
            if (!PhotonNetwork.IsMasterClient) return;
            Debug.Log("TurnEnd");

            BattleLogic.TurnProcessed++;
            Ready1 = false;
            Ready2 = false;

            PrePhase ();
        }

        private void SwitchGameState () {
            if (!PhotonNetwork.IsMasterClient) return;
            //Debug.Log(GameState);

            if (GameState == GameState.WaitingForInput) {
                RemainingTime -= Time.fixedDeltaTime;
                // 時間が過ぎるか入力が終わったらEndPhase
                if (RemainingTime <= 0) {
                    EndPhase ();
                }
                if (Ready1 == true && Ready2 == true) {
                    EndPhase ();
                }
            } else if (GameState == GameState.Matching) {
                if (MasterPlayer != null && GuestPlayer != null) {
                    PrePhase ();
                }
            } else if (GameState == GameState.TurnStart) {
                if (Ready1 && Ready2) {
                    TurnStart ();
                }
            } else if (GameState == GameState.TurnEnd) {
                if (Ready1 && Ready2) {
                    PrePhase();
                }
            }
        }
    }
}