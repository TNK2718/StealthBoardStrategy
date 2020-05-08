using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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

        private Board Board;
        private List<ClientUnit> UnitList1;
        private List<ClientUnit> UnitList2;

        // ボードとキャラを同期
        // サーバー側からRPCする
        [PunRPC]
        public void SyncBoard (string msg, string boardJson, string unitListJson1, string unitListJson2, PhotonMessageInfo info) {
            Board board = JsonUtility.FromJson<Board> (boardJson);
            ClientUnit[] unitList1 = JsonUtility.FromJson<ClientUnit[]> (unitListJson1);
            ClientUnit[] unitList2 = JsonUtility.FromJson<ClientUnit[]> (unitListJson2);
            Board = board;
            UnitList1 = new List<ClientUnit> (unitList1.Length);
            for (int i = 0; i < unitList1.Length; i++) {
                UnitList1[i] = unitList1[i];
            }
            UnitList2 = new List<ClientUnit> (unitList2.Length);
            for (int i = 0; i < unitList2.Length; i++) {
                UnitList2[i] = unitList2[i];
            }
            Debug.Log(UnitList1[0].Hp);
        }

        private void Awake () {
            if (photonView.IsMine) {
                LocalPlayerInstance = this.gameObject;
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