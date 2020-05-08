namespace StealthBoardStrategy.Server.DataBase {
    public class Buff {
        public const int INF = 10000000;
        public BuffType Type;
        public int Power;
        public int Duration;

        public Buff(BuffType buffType, int power, int duration){
            Type = buffType;
            Power = power;
            Duration = duration;
        }
    }
}