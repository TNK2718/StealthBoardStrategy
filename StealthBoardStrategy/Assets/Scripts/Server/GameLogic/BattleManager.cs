using System;
using System.Collections.Generic;
using Photon.Pun;
using StealthBoardStrategy.Frontend.Client;
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
        private List<Unit> UnitList1;
        private List<Unit> UnitList2;
        private Board Board;

        public GameObject MasterPlayer;
        public GameObject GuestPlayer;

        private void Start () {

            // test
            UnitList1 = new List<Unit> { new Unit (0, Players.Player1, 0, 0, Players.Player1) };
            UnitList2 = new List<Unit> { new Unit (0, Players.Player2, 0, 0, Players.Player2) };
            SyncBoard ();
        }

        [PunRPC]
        public void SendActionMessage () {

        }

        // クライアント側にボードを送信
        public void SyncBoard () {
            try {
                string boardJson = JsonUtility.ToJson (Board);
                ClientUnit[] unitList11 = new ClientUnit[UnitList1.Count];
                ClientUnit[] unitList12 = new ClientUnit[UnitList1.Count];
                ClientUnit[] unitList21 = new ClientUnit[UnitList2.Count];
                ClientUnit[] unitList22 = new ClientUnit[UnitList2.Count];
                for (int i = 0; i < UnitList1.Count; i++) {
                    unitList11[i] = UnitList1[i].ConvertToClientUnit (false);
                    unitList12[i] = UnitList1[i].ConvertToClientUnit (true);
                    unitList21[i] = UnitList2[i].ConvertToClientUnit (true);
                    unitList22[i] = UnitList2[i].ConvertToClientUnit (false);
                }
                string unitListJson11 = JsonUtility.ToJson (unitList11);
                string unitListJson12 = JsonUtility.ToJson (unitList12);
                string unitListJson21 = JsonUtility.ToJson (unitList21);
                string unitListJson22 = JsonUtility.ToJson (unitList22);
                object[] args1 = new object[] {
                    "SyncBoard",
                    boardJson,
                    unitListJson11,
                    unitListJson21,
                };
                object[] args2 = new object[] {
                    "SyncBoard",
                    boardJson,
                    unitListJson12,
                    unitListJson22,
                };
            
            MasterPlayer.GetComponent<PhotonView> ().RPC ("SyncBoard", RpcTarget.AllViaServer, args1);
            GuestPlayer.GetComponent<PhotonView> ().RPC ("SyncBoard", RpcTarget.AllViaServer, args2);
            }
            catch {

            }
        }
        public void BattleLoop () {
            if (!PhotonNetwork.IsMasterClient) return;
        }
        private void TurnStart () {

        }
        private void PrePhase () {
            if (!PhotonNetwork.IsMasterClient) return;
        }
        // クライアントから送られてきたActionEventを受け取って処理
        private void ActionPhase (ActionEvent actionEvent) {
            if (!PhotonNetwork.IsMasterClient) return;
            // TODO: プレイヤーの識別, 認証
            /*switch (UnitList[actionEvent.Invoker].SkillList[actionEvent.ActionNo].SkillType) {
                case SkillType.Move:
                    break;
                case SkillType.Attack:
                    break;
                default:
                    break;
            }*/
        }
        private void EndPhase () {
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