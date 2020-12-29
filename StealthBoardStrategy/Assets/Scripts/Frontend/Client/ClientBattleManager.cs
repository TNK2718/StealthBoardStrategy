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
using UnityEngine.Tilemaps;

namespace StealthBoardStrategy.Frontend.Client {
    // クライアント上でゲームの情報を管理する
    // プレイヤーキャラクターとしてGameManagerから生成される
    public class ClientBattleManager : MonoBehaviourPunCallbacks {
        public static GameObject LocalPlayerInstance;
        public const int MaxUnits = 3;

        private GameObject Master;
        private GameObject CameraObj;
        private Tilemap BoardTileMap;

        private Players MyPlayer;
        private Players EnemyPlayer;
        private Board Board;
        private List<ClientUnit> UnitList1;
        private List<ClientUnit> UnitList2;
        private ClientGameState GameState;
        // 選択した行動をサーバーへ送る
        private ActionEvent ActionEvent;
        private (Players player, int index) SelectedUnit;

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
                // エフェクト等の処理が終わるのを待ってサーバーに通知
            } else if (msg == "TurnEndEventToClient") {
                // エフェクトとか
            } else {
                Debug.LogAssertion ("Error");
            }
            // 処理が終わったことを通知(全てReadyEventに統一)
            object[] args = new object[] { "ReadyEvent", JsonUtility.ToJson (new ReadyEvent ()) };
            Master.GetComponent<PhotonView> ().RPC ("SendEventToMaster", RpcTarget.MasterClient, args);
        }

        private void Awake () {
            Board = new Board ();
            UnitList1 = new List<ClientUnit> (MaxUnits);
            UnitList2 = new List<ClientUnit> (MaxUnits);
            if (PhotonNetwork.IsMasterClient) {
                MyPlayer = Players.Player1;
                EnemyPlayer = Players.Player2;
            } else {
                MyPlayer = Players.Player2;
                EnemyPlayer = Players.Player1;
            }
            SelectedUnit = (Players.None, -1);
        }

        private void Start () {
            GameState = ClientGameState.Matching;
            // カメラを取得
            CameraObj = Camera.allCameras[0].gameObject;
            RegisterPlayerToBattleManager ();
            // Tilemapを取得
            BoardTileMap = GameObject.Find ("BoardTilemap").GetComponent<Tilemap> ();
        }

        private void FixedUpdate () {
            RecieveTileInput ();
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

        // タイルへのクリック入力を処理
        private void RecieveTileInput () {
            if (!photonView.IsMine) return;
            if (Input.GetMouseButtonDown (0)) {
                // Vector3でマウス位置座標を取得する
                var position = Input.mousePosition;
                // Z軸修正
                position.z = 10f;
                // マウス位置座標をスクリーン座標からワールド座標に変換する
                var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint (position);
                //tilemap座標を得る
                Vector3Int clickPosition = BoardTileMap.WorldToCell (screenToWorldPointPosition);
                if (BoardTileMap.HasTile (clickPosition) == true) {
                    Debug.Log (clickPosition);
                }

                // 選択したtilemapに対して処理
                if (GameState == ClientGameState.WaitingForInput) {
                    SelectUnit (clickPosition);
                }
            }
        }
        // クリックしたユニットにフォーカス
        private void SelectUnit (Vector3Int _clickPos) {
            List<int> units;
            if (_clickPos.y > 0) {
                // 敵陣をクリック
                units = SearchUnit (EnemyPlayer, _clickPos.x, _clickPos.y);
                // 誰もいなかったら操作しない
                if (units.Count == 0) return;
                SelectedUnit.player = EnemyPlayer;
                // 同じ位置に複数ユニットいる場合順番にフォーカス
                if (SelectedUnit.player == EnemyPlayer && units.Find (x => x == SelectedUnit.index) >= 0 && units.Find (x => x == SelectedUnit.index) < units.Count - 1) {
                    SelectedUnit.index = units.Find (x => x == SelectedUnit.index) + 1;
                } else {
                    SelectedUnit.index = units[0];
                }
            } else if (_clickPos.y < 0) {
                // 自陣をクリック
                units = SearchUnit (MyPlayer, _clickPos.x, _clickPos.y);
                // 誰もいなかったら操作しない
                if (units.Count == 0) return;
                SelectedUnit.player = MyPlayer;
                // 同じ位置に複数ユニットいる場合順番にフォーカス
                if (SelectedUnit.player == MyPlayer && units.Find (x => x == SelectedUnit.index) >= 0 && units.Find (x => x == SelectedUnit.index) < units.Count - 1) {
                    SelectedUnit.index = units.Find (x => x == SelectedUnit.index) + 1;
                } else {
                    SelectedUnit.index = units[0];
                }
            }
        }
        // ある位置にいるユニットを列挙
        private List<int> SearchUnit (Players player, int x, int y) {
            List<int> result = new List<int> ();
            if (player == Players.Player1) {
                for (int i = 0; i < UnitList1.Count; i++) {
                    if (UnitList1[i].PositionX == x && UnitList1[i].PositionY == y) {
                        result.Add (i);
                    }
                }
            } else if (player == Players.Player2) {
                for (int i = 0; i < UnitList2.Count; i++) {
                    if (UnitList2[i].PositionX == x && UnitList2[i].PositionY == y) {
                        result.Add (i);
                    }
                }
            } else {
                Debug.LogWarning ("Error");
            }
            return result;
        }
        private List<ClientUnit> GetUnitList (Players player) {
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