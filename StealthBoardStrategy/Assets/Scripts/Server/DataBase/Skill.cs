using System;
using System.IO;
using System.Text;

namespace StealthBoardStrategy.Server.DataBase {
    public class Skill {
        public int Id;
        public string Name;
        public SkillType SkillType;
        public RangeType RangeType;
        public int[] args;
    }
}