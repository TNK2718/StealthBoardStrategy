using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StealthBoardStrategy.Server.DataBase {

    public class SkillList {
        public const int ARGSNUM = 10;
        public List<Skill> Skills;
        public SkillList () {
            try {
                string filePath = @"Assets/Scripts/Server/DataBase/SkillData.csv";
                var fs = new FileStream (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader (fs, Encoding.GetEncoding ("Shift_JIS"));
                int count = 0;
                while (reader.Peek () >= 0) {
                    int tmp = 0;
                    string[] cols = reader.ReadLine ().Split (',');
                    Skills[count] = new Skill ();
                    Skills[count].Id = int.Parse(cols[tmp++]);
                    Skills[count].args = new int[ARGSNUM];
                    Skills[count].Name = cols[tmp++];
                    Skills[count].SkillType = (SkillType) Enum.Parse (typeof (SkillType), cols[tmp++]);
                    Skills[count].RangeType = (RangeType) Enum.Parse (typeof (RangeType), cols[tmp++]);
                    for (int i = 0; 3 + 1 + i < cols.Length && i < ARGSNUM; i++) {
                        int.TryParse (cols[3 + 1 + i], out Skills[count].args[i]);
                    }
                    count++;
                    reader.ReadLine ();
                }
                reader.Close ();
            } catch {

            }
        }
    }
}