using StealthBoardStrategy.Server.DataBase;

namespace StealthBoardStrategy.Server.GameLogic
{
    public enum GameState
    {
        None, Matching, WaitingForInput, AccepetedInput, TurnStart, TurnEnd,
    }
}