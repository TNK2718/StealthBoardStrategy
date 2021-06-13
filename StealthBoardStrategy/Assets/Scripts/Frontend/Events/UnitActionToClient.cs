using System.Collections;
using System.Collections.Generic;
using StealthBoardStrategy.Frontend.Graphic;
using UnityEngine;
using System;


namespace StealthBoardStrategy.Frontend.Events {
    [Serializable]
    public class UnitActionToClient {
        public int InvokerPositionX;
        public int InvokerPositionY;
        public int TargetPositionX;
        public int TargetPositionY;
        public int[] args;
        public EffectType EffectType;

    }
}