using StealthBoardStrategy.Server.DataBase;

namespace StealthBoardStrategy.Server.GameLogic
{
    public class GroundTile
    {
        public int Hp;
        public BuildingType BuildingType;
        public Players Owner;

        public GroundTile(){
            Hp = 0;
            BuildingType = BuildingType.None;
            Owner = Players.None;
        }

        public void SetHp(int value){
            if(value >= 0){
                Hp = value;
            } else{
                Hp = 0;
            }
        }

        public void ConditionUpdate(){
            if(Hp <= 0){
                BuildingType = BuildingType.None;
                Owner = Players.None;
            }
        }
    }
}