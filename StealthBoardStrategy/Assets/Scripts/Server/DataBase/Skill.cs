namespace StealthBoardStrategy.Server.DataBase {
    public class Skill {
        public (int value, bool visibility) Id;
        public (string value, bool visibility) Name;
        public (int value, bool visibility) Power;
        public SkillType SkillType;
        public Skill(int id){

        }
    }
}