using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using StealthBoardStrategy.Frontend.Events;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StealthBoardStrategy.Frontend.Client {
    // クライアント上でゲームの情報を管理する
    // プレイヤーキャラクターとしてGameManagerから生成される
    public class ClientBattleManager : MonoBehaviourPun {
        public static GameObject LocalPlayerInstance;
        public const int MaxUnits = 3;
        [SerializeField]
        private GameObject Master;

        private Players Turn;
        private Board Board;
        private List<ClientUnit> UnitList1;
        private List<ClientUnit> UnitList2;

        // ボードとキャラを同期
        [PunRPC]
        public void SyncBoard (string msg, string boardJson, string[] unitListJson1, string[] unitListJson2, PhotonMessageInfo info) {
            try {
                Board board = JsonUtility.FromJson<Board> (boardJson);
                Board = board;
                UnitList1.Clear ();
                for (int i = 0; i < unitListJson1.Length; i++) {
                    UnitList1.Add (JsonUtility.FromJson<ClientUnit> (unitListJson1[i]));
                }
                UnitList2.Clear ();
                for (int i = 0; i < unitListJson2.Length; i++) {
                    UnitList2.Add (JsonUtility.FromJson<ClientUnit> (unitListJson2[i]));
                }
            } catch {
                Debug.Log ("SyncBoardError");
            }
        }

        // サーバーからEventを受け取って処理
        [PunRPC]
        public void ReceiveEvent (string msg, GameEventToClient gameEventToClient) {
            if (gameEventToClient.GetType () == typeof (ActionEventToClient)) {
                // エフェクトとか

            } else {

            }
            // 処理が終わったことを通知
            object[] args = new object[]{"SendEventToMaster", gameEventToClient};
            Master.GetComponent<PhotonView> ().RPC ("SendEventToMaster", RpcTarget.MasterClient, args);
        }

        private void Awake () {
            BattleManager battleManager = Master.GetComponent<BattleManager> ();
            if (photonView.IsMine) {
                LocalPlayerInstance = this.gameObject;
                if (PhotonNetwork.IsMasterClient) battleManager.MasterPlayer = this.gameObject; // battleManagerにプレイヤーを登録
                else battleManager.GuestPlayer = this.gameObject;
            } else {
                if (PhotonNetwork.IsMasterClient) battleManager.GuestPlayer = this.gameObject;
                else battleManager.MasterPlayer = this.gameObject;
            }
            DontDestroyOnLoad (this.gameObject);
        }

        private void Start () {
            Board = new Board ();
            UnitList1 = new List<ClientUnit> (MaxUnits);
            UnitList2 = new List<ClientUnit> (MaxUnits);
        }
    }
}