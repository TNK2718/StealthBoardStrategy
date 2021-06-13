using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using StealthBoardStrategy.Frontend.Client;
using StealthBoardStrategy.Frontend.Events;
using StealthBoardStrategy.Frontend.Graphic;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;

namespace StealthBoardStrategy.Server.GameLogic {
    public class BattleLogic {
        public int TurnProcessed;

        public List<Unit> UnitList1;
        public List<Unit> UnitList2;
        public Board Board;

        public BattleLogic () {
            TurnProcessed = 0;
            Board = new Board ();
            // プレイヤーユニットを生成
            UnitList1 = new List<Unit> { new Unit (0, Players.Player1, 0, -1, Players.Player1), new Unit (0, Players.Player1, 0, -1, Players.Player1), new Unit (0, Players.Player1, 0, -1, Players.Player1) };
            UnitList2 = new List<Unit> { new Unit (0, Players.Player2, 0, 1, Players.Player2), new Unit (0, Players.Player2, 0, 1, Players.Player2), new Unit (0, Players.Player2, 0, 1, Players.Player2) };
        }

        public object[][] SerializeBoard () {
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
                object[][] argsarray = new object[][] { args1, args2 };
                return argsarray;
            } catch {
                Debug.LogWarning ("SerializingError");
            }
            return null;
        }

        // プレイヤーの行動を処理してクライアントへエフェクトの表示等をさせる命令を返す
        public ActionEventToClient ProcessActionEvents (ActionEvent unitAction1, ActionEvent unitAction2) {
            if (unitAction1.UnitActions.Length == 0 && unitAction2.UnitActions.Length == 0) {
                return new ActionEventToClient ();
            }
            ActionEventToClient actionEventToClient = new ActionEventToClient ();
            List<UnitActionToClient> unitActionsToClient = new List<UnitActionToClient> ();

            // Agility順に各ユニットの行動をソート
            List<UnitAction> concatUnitActions = new List<UnitAction> ();
            if (unitAction1 != null) {
                for (int i = 0; i < unitAction1.UnitActions.Length; i++) {
                    concatUnitActions.Add (unitAction1.UnitActions[i]);
                }
            }
            if (unitAction2 != null) {
                for (int i = 0; i < unitAction2.UnitActions.Length; i++) {
                    concatUnitActions.Add (unitAction2.UnitActions[i]);
                }
            }
            concatUnitActions.Sort ((a, b) => b.Agility - a.Agility);

            // 各行動を処理
            for (int i = 0; i < concatUnitActions.Count; i++) {
                // skillTypeで分岐
                if (concatUnitActions[i].ActionNo == 0) {
                    // Move
                    unitActionsToClient.Add (Move (concatUnitActions[i].Owner, concatUnitActions[i].Invoker, (concatUnitActions[i].TargetPositionX, concatUnitActions[i].TargetPositionY)));
                } else if(concatUnitActions[i].ActionNo <= -1) {
                    // 通常攻撃
                    Debug.Log("normal attack");
                } else if (GetUnitList (concatUnitActions[i].Owner) [concatUnitActions[i].Invoker].SkillList[concatUnitActions[i].ActionNo].SkillType == SkillType.Attack) {
                    // Attack todo

                } else {
                    // 空のアクション
                    Debug.Log ("None");
                    unitActionsToClient.Add (None ());
                }
            }

            // ActionEventToClientを返す
            Debug.Log (unitActionsToClient.Count);
            actionEventToClient.UnitActions = new UnitActionToClient[unitActionsToClient.Count];
            for (int i = 0; i < unitActionsToClient.Count; i++) {
                actionEventToClient.UnitActions[i] = unitActionsToClient[i];
            }

            return actionEventToClient;
        }

        //
        // スキルの実装
        // ActionEventToClientを返す

        // 移動
        private UnitActionToClient Move (Players owner, int invoker, (int x, int y) destination) {
            Unit unit = GetUnitList (owner) [invoker];
            float squareddistanse = destination.x * destination.x + destination.y * destination.y;
            if (unit.GetSpeed () * unit.GetSpeed () < squareddistanse || destination.y == 0) {
                // 不正な入力
                Debug.Log ("Invalid input by unit" + invoker.ToString () + " on" + owner.ToString ());
                return null;
            }

            UnitActionToClient unitAction = new UnitActionToClient ();
            unitAction.InvokerPositionX = unit.GetPosition ().x;
            unitAction.InvokerPositionY = unit.GetPosition ().y;

            // 移動
            unit.SetPosition (destination.x, destination.y);

            unitAction.TargetPositionX = destination.x;
            unitAction.TargetPositionY = destination.y;
            unitAction.args[0] = (int) owner;
            unitAction.args[1] = invoker;
            unitAction.EffectType = EffectType.Move;
            return unitAction;
        }

        private void Attack () {

        }

        private UnitActionToClient None () {
            UnitActionToClient unitAction = new UnitActionToClient ();
            unitAction.EffectType = EffectType.None;
            return unitAction;
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