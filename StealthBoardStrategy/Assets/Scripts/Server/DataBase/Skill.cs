using System.IO;
using System.Text;
using System;

namespace StealthBoardStrategy.Server.DataBase {
    public class Skill {
        public const int ARGSNUM = 5;
        public int Id;
        public string Name;
        public SkillType SkillType;
        public int[] args;

        public Skill(int id){
            string filePath = @"Assets/Scripts/Server/DataBase/SkillData.csv";
            StreamReader reader = new StreamReader (filePath, Encoding.GetEncoding ("Shift_JIS"));
            int count = 0;
            while (reader.Peek () >= 0) {
                if(count == id){
                    string[] cols = reader.ReadLine().Split(',');
                    Name = cols[1];
                    SkillType = (SkillType) Enum.Parse(typeof(SkillType), cols[2]);
                    for(int i = 0; i < ARGSNUM; i++){
                        int.TryParse(cols[i], out args[i]);
                    }
                    break;
                }
                reader.ReadLine();
            }
        }
    }
}