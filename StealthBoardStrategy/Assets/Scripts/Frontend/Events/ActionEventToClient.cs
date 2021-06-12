using StealthBoardStrategy.Frontend.Graphic;
using System.Collections.Generic;
using System;

namespace StealthBoardStrategy.Frontend.Events
{
    [Serializable]
    public class ActionEventToClient: GameEventToClient
    {
        public UnitActionToClient[] UnitActions;
    }
}