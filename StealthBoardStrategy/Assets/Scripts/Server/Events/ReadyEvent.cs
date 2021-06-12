using UnityEngine;
using Photon.Pun;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;
using System;

namespace StealthBoardStrategy.Server.Events
{
    [Serializable]
    public class ReadyEvent: GameEvent
    {
        public ReadyEvent(Players sender){
            Sender = sender;
        }
    }
}