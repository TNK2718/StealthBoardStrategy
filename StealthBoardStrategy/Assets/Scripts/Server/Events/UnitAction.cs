using Photon.Pun;
using UnityEngine;
using StealthBoardStrategy.Server.GameLogic;

namespace StealthBoardStrategy.Server.Events {
    public class UnitAction {
        public Players Owner;
        public int Invoker;
        public int Agility;
        // 絶対座標
        public int TargetPositionX;
        public int TargetPositionY;
        // 0: Move, 1: Skill1, 2: Skill2, ...
        public int ActionNo;
        public int[] Parameters;
    }
}