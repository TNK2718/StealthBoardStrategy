using UnityEngine;
using Photon.Pun;
using System;

namespace StealthBoardStrategy.Server.Events
{
    [Serializable]
    public class ActionEvent: GameEvent
    {
        public UnitAction[] UnitActions;
    }
}