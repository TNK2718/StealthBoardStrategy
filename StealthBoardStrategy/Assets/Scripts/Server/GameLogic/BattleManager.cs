using System;
using System.Collections.Generic;
using Photon.Pun;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;

namespace StealthBoardStrategy.Server.GameLogic {
    [RequireComponent (typeof (PhotonView))]
    public class BattleManager : MonoBehaviourPunCallbacks {
        private int TurnProcessed;
        private float RemainingTime;
        private Players Turn;
        private List<Unit> UnitList;
        private Board Board;

        private PhotonView MasterView;
        private PhotonView OtherView;

        private void Start () {
            //SyncBoard ();
        }

        [PunRPC]
        public void SendActionMessage () {

        }

        // クライアント側にボードを送信
        public void SyncBoard () {
            string boardJson = JsonUtility.ToJson (Board);
            Unit[] unitList = new Unit[UnitList.Count];
            for (int i = 0; i < UnitList.Count; i++) {
                unitList[i] = UnitList[i];
            }
            string unitListJson = JsonUtility.ToJson (unitList);
            object[] args = new object[] {
                "SyncBoard",
                boardJson,
                unitListJson
            };
            MasterView.RPC ("SyncBoard", RpcTarget.AllViaServer, args);
            OtherView.RPC ("SyncBoard", RpcTarget.AllViaServer, args);
        }
        public void BattleLoop () {
            if (!PhotonNetwork.IsMasterClient) return;
        }
        private void TurnStart () {

        }
        private void PreProcess () {
            if (!PhotonNetwork.IsMasterClient) return;
        }
        // クライアントから送られてきたActionEventを受け取って処理
        private void ActionProcess (ActionEvent actionEvent) {
            if (!PhotonNetwork.IsMasterClient) return;
            // TODO: プレイヤーの識別, 認証
            switch (UnitList[actionEvent.Invoker].SkillList[actionEvent.ActionNo].SkillType) {
                case SkillType.Move:
                    break;
                case SkillType.Attack:
                    break;
                default:
                    break;
            }
        }
        private void EndProcess () {
            if (!PhotonNetwork.IsMasterClient) return;
        }
        public void TurnEnd () {
            if (!PhotonNetwork.IsMasterClient) return;
        }

        //
        // スキルの実装
        //
        private void Move (Unit invoker, (int x, int y) position) {
            float squareddistanse = (invoker.Position.x - position.x) * (invoker.Position.x - position.x) + (invoker.Position.y - position.y) * (invoker.Position.y - position.y);
            if (invoker.GetSpeed () * invoker.GetSpeed () >= squareddistanse) {
                invoker.Position = (position.x, position.y, invoker.Position.visibility);
            } else {
                // 不正な入力を通知
            }
        }
        private void Attack () {

        }
    }
}