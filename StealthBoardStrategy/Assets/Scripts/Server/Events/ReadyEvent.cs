using UnityEngine;
using Photon.Pun;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;

namespace StealthBoardStrategy.Server.Events
{
    public class ReadyEvent: GameEvent
    {
        public ReadyEvent(Players sender){
            Sender = sender;
        }
    }
}