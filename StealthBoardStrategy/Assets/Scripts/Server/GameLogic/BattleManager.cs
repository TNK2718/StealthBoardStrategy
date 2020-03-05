using System;
using System.Collections.Generic;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;
using Photon.Pun;

namespace StealthBoardStrategy.Server.GameLogic
{
    public class BattleManager: MonoBehaviourPunCallbacks
    {
        private int TurnProcessed;
        private float RemainingTime;
        private Players Turn;
        private List<Unit> UnitList;
        private Board Board;

        public void BattleLoop(){

        }

        public void PreProcess(){

        }
        // クライアントから送られてきたActionEventを受け取って処理
        public void ActionProcess(ActionEvent actionEvent){
            // TODO: プレイヤーの識別, 認証
            switch(UnitList[actionEvent.Invoker].SkillList[actionEvent.ActionNo].SkillType){
                case SkillType.Move:
                    break;
                case SkillType.Attack:
                    break;
                default:
                    break;
            }
        }
        public void EndProcess(){

        }
        public void TurnEnd(){

        }
        
        //
        // スキルの実装
        //
        private void Move(Unit invoker, (int x, int y) position){
            float squareddistanse = (invoker.Position.x - position.x) * (invoker.Position.x - position.x) + (invoker.Position.y - position.y) * (invoker.Position.y - position.y);
            if(invoker.GetSpeed() * invoker.GetSpeed() >= squareddistanse){
                invoker.Position = (position.x, position.y, invoker.Position.visibility);
            }else{
                // 不正な入力を通知
            }
        }
        private void Attack(){
            
        }
    }
}