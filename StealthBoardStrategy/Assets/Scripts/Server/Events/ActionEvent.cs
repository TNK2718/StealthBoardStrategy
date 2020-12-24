using UnityEngine;
using Photon.Pun;

namespace StealthBoardStrategy.Server.Events
{
    public class ActionEvent: GameEvent
    {
        public UnitAction[] UnitActions;
    }
}