using System.Collections;
using System.Collections.Generic;
using StealthBoardStrategy.Frontend.Graphic;
using UnityEngine;

namespace StealthBoardStrategy.Frontend.Events {
    public class UnitActionToClient {
        public int InvokerPositionX;
        public int InvokerPositionY;
        public int TargetPositionX;
        public int TargetPositionY;
        public List<int> args;
        public EffectType EffectType;

    }
}