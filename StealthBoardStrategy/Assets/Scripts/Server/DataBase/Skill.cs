using System;
using System.IO;
using System.Text;

namespace StealthBoardStrategy.Server.DataBase {
    public class Skill {
        public const int ARGSNUM = 5;
        public int Id;
        public string Name;
        public SkillType SkillType;
        public int[] args;

        public Skill (int id) {
            string filePath = @"Assets/Scripts/Server/DataBase/SkillData.csv";
            var fs = new FileStream (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader (fs, Encoding.GetEncoding ("Shift_JIS"));
            int count = 0;
            Id = id;
            args = new int[ARGSNUM];
            while (reader.Peek () >= 0) {
                if (count == id) {
                    string[] cols = reader.ReadLine ().Split (',');
                    Name = cols[1];
                    SkillType = (SkillType) Enum.Parse (typeof (SkillType), cols[2]);
                    for (int i = 0; 2 + 1 + i < cols.Length && i < ARGSNUM; i++) {
                        int.TryParse (cols[2 + 1 + i], out args[i]);
                    }
                    break;
                }
                reader.ReadLine ();
            }
            reader.Close();
        }
    }
}