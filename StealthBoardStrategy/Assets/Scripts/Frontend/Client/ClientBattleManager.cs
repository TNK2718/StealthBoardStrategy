using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using StealthBoardStrategy.Frontend.Events;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StealthBoardStrategy.Frontend.Client {
    // クライアント上でゲームの情報を管理する
    // プレイヤーキャラクターとしてGameManagerから生成される
    public class ClientBattleManager : MonoBehaviourPunCallbacks {
        public static GameObject LocalPlayerInstance;
        public const int MaxUnits = 3;
        private GameObject Master;
        private GameObject CameraObj;

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
        public void RecieveEvent (string msg, string gameEventToClientJson) {
            Debug.Log (msg);
            GameEventToClient gameEventToClient = null;
            if (msg == "ActionEventToClient") {
                gameEventToClient = JsonUtility.FromJson<ActionEventToClient> (gameEventToClientJson);
                // エフェクトとか

            } else if (msg == "TrunStartEventToClient") {
                // エフェクトとか
            } else if (msg == "TurnEndEventToClient") {
                // エフェクトとか
            } else {
                Debug.LogAssertion ("Error");
            }
            // 処理が終わったことを通知
            object[] args = new object[] { "ReadyEvent", JsonUtility.ToJson (new ReadyEvent ()) };
            Master.GetComponent<PhotonView> ().RPC ("SendEventToMaster", RpcTarget.MasterClient, args);
        }

        private void Awake () {
            Board = new Board ();
            UnitList1 = new List<ClientUnit> (MaxUnits);
            UnitList2 = new List<ClientUnit> (MaxUnits);
        }

        private void Start () {
            // カメラを取得
            CameraObj = Camera.allCameras[0].gameObject;
            RegisterPlayerToBattleManager ();
        }

        private void FixedUpdate () {

        }

        // 初期化
        private void RegisterPlayerToBattleManager () {
            Master = GameObject.Find ("Master");
            BattleManager battleManager = Master.GetComponent<BattleManager> ();
            if (this.photonView.IsMine) {
                LocalPlayerInstance = this.gameObject;
                if (PhotonNetwork.IsMasterClient) battleManager.MasterPlayer = this.gameObject; // battleManagerにプレイヤーを登録
                else battleManager.GuestPlayer = this.gameObject;
            } else {
                if (PhotonNetwork.IsMasterClient) battleManager.GuestPlayer = this.gameObject;
                else battleManager.MasterPlayer = this.gameObject;
            }
            DontDestroyOnLoad (this.gameObject);
        }

        // 入力を処理
        private void RecieveInput () {
            Vector3 clickPos;
            Vector3 position;
            Vector3 direction;
            if (Input.GetMouseButtonDown (0)) {
                clickPos = Input.mousePosition;
                position = CameraObj.transform.position;
                direction = Camera.main.ScreenToWorldPoint(clickPos) - CameraObj.transform.position;
                //Physics.Raycast(position, direction, 100);
                Debug.DrawRay(position, direction* 100, Color.red, 1);
            }
        }
    }
}