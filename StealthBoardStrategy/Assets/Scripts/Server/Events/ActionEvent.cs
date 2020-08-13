using UnityEngine;
using Photon.Pun;

namespace StealthBoardStrategy.Server.Events
{
    public class ActionEvent: GameEvent
    {
        public int Invoker;
        public int TargetPositionX;
        public int TargetPositionY;
        // 0: Move, 1: Skill1, 2: Skill2, ...
        public int ActionNo;
        public int[] Parameters;
    }
}