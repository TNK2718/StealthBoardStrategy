using UnityEngine;
using Photon.Pun;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;

namespace StealthBoardStrategy.Server.Events
{
    public abstract class GameEvent
    {
        public Players Sender;
    }
}