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
        private Players Turn;
        private GameState GameState;
        private BattleLogic BattleLogic;
        private List<ActionEvent> ActionEvents;

        public GameObject MasterPlayer;
        public GameObject GuestPlayer;

        private void Start () {
            // ここからはMasterClientのみ
            if (!PhotonNetwork.IsMasterClient) return;
            GameState = GameState.Matching;
            Turn = Players.Player1;
        }

        private void FixedUpdate () {
            if (PhotonNetwork.IsMasterClient) Debug.Log (GameState);
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
                gameEvent = JsonUtility.FromJson<ActionEvent> (gameEventJson);
                RespondToActionEvent ((ActionEvent) gameEvent);
            } else if (msg == "ReadyEvent") {
                RespondToReadyEvent ();
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

        // ターン開始前の処理
        private void PrePhase () {
            if (!PhotonNetwork.IsMasterClient) return;
            GameState = GameState.TurnStart;
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
            RemainingTime = SELECTING_TIME;

            // 入力を受付
            GameState = GameState.WaitingForInput;
        }
        // クライアントから送られてきたActionEventを受け取って処理
        // TODO: プレイヤーの識別, 認証
        private void RespondToActionEvent (ActionEvent actionEvent) {
            if (!PhotonNetwork.IsMasterClient) return;
            if (!(GameState == GameState.WaitingForInput)) return;
            if (actionEvent.Sender != Turn) return;

            // クライアントと同期
            SyncBoardToClients ();
        }

        // ReadyEventに対する対応
        private void RespondToReadyEvent () {
            if (!PhotonNetwork.IsMasterClient) return;
            if (GameState == GameState.Matching) {

            } else if (GameState == GameState.TurnStart) {
                // クライアント側の準備が出来たのでターン開始
                TurnStart ();
            } else if (GameState == GameState.TurnEnd) {
                // 次のターンへ移行
                TurnEnd ();
            } else {

            }
        }
        // ターン終了時の処理
        // dotダメージや建造物の効果などはEndPhaseで発動する
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
    }
}