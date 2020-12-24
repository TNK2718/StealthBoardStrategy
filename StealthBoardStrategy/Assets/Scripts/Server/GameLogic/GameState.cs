using StealthBoardStrategy.Server.DataBase;

namespace StealthBoardStrategy.Server.GameLogic
{
    public enum GameState
    {
        None, Matching, WaitingForInput, TurnStart, TurnEnd, GameEnd
    }
}