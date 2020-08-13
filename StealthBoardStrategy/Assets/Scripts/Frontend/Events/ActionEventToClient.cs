using StealthBoardStrategy.Frontend.Graphic;

namespace StealthBoardStrategy.Frontend.Events
{
    public class ActionEventToClient: GameEventToClient
    {
        int InvokerPositionX;
        int InvokerPositionY;
        int TargetPositionX;
        int TargetPositionY;
        EffectType EffectType;
    }
}