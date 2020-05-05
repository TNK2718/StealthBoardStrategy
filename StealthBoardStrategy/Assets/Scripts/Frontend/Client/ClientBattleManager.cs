using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using StealthBoardStrategy.Server.GameLogic;
using StealthBoardStrategy.Server.DataBase;

namespace StealthBoardStrategy.Frontend.Client {
    // クライアント上でゲームの情報を管理する
    // プレイヤーキャラクターとしてGameManagerから生成される
    public class ClientBattleManager : MonoBehaviourPun {
        public static GameObject LocalPlayerInstance;

        private Board Board;
        private List<ClientUnit> UnitList;

        [PunRPC]
        public void SyncBoard(string msg, string boardJson, string unitListJson, PhotonMessageInfo info){
            Board board = JsonUtility.FromJson<Board>(boardJson);
            ClientUnit[] unitList = JsonUtility.FromJson<ClientUnit[]>(unitListJson);
            Board = board;
            for(int i = 0; i < unitList.Length; i++){
                UnitList[i] = unitList[i];
            }
        }

        private void Awake() {
            if(photonView.IsMine){
                LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start() {
            Board = new Board();
            UnitList = new List<ClientUnit>(6);
        }
    }
}