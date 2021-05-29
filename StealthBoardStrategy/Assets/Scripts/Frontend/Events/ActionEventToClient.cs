using StealthBoardStrategy.Frontend.Graphic;
using System.Collections.Generic;


namespace StealthBoardStrategy.Frontend.Events
{
    public class ActionEventToClient: GameEventToClient
    {
        public UnitActionToClient[] UnitActions;
    }
}