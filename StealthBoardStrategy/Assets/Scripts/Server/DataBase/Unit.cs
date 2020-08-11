using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StealthBoardStrategy.Frontend.Client;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;

namespace StealthBoardStrategy.Server.DataBase {
    public class Unit {
        public (int value, bool visibility) Id;
        protected (string value, bool visibility) Name;
        public Players LocatedSide;
        private (int x, int y, bool visibility) Position;
        private (int baseVal, int diff, bool visibility) Hp;
        public (int baseVal, int diff, bool visibility) MaxHp;
        public (int baseVal, int diff, bool visibility) HpRegen;
        private (int baseVal, int diff, bool visibility) Shield;
        public (int baseVal, int diff, bool visibility) MaxShield;
        public (int baseVal, int diff, bool visibility) ShieldRegen;
        private (int baseVal, int diff, bool visibility) Mana;
        public (int baseVal, int diff, bool visibility) MaxMana;
        public (int baseVal, int diff, bool visibility) ManaRegen;
        public (int baseVal, int diff, bool visibility) Stealthiness;
        public (int baseVal, int diff, bool visibility) MaxStealthiness;
        public (int baseVal, int diff, bool visibility) StealthRegen;
        public (int baseVal, int diff, bool visibility) Atk;
        public (int baseVal, int diff, bool visibility) Mp;
        public (int baseVal, int diff, bool visibility) Def;
        public (int baseVal, int diff, bool visibility) MR;
        public (int baseVal, int diff, bool visibility) Speed;
        public (int baseVal, int diff, bool visibility) Calculation;
        public Players Owner;
        public List<Skill> SkillList;
        public List<Buff> BuffList;
        public List<int> DemonList; // Demon's ID
        public UnitType UnitType;
        public int ActionPoint;
        protected bool IsActive;

        public (int value, bool visibility) PrimaryDemon;

        public (int x, int y) GetPosition(){
            return (Position.x, Position.y);
        }
        public int GetHp () {
            if (Hp.baseVal + Hp.diff >= 0) return Hp.baseVal + Hp.diff;
            else return 0;
        }
        public int GetMaxHp () {
            if (MaxHp.baseVal + MaxHp.diff >= 0) return MaxHp.baseVal + Hp.diff;
            else return 0;
        }
        public int GetHpRegen () {
            if (HpRegen.baseVal + HpRegen.diff >= 0) return HpRegen.baseVal + HpRegen.diff;
            else return 0;
        }
        public int GetMana () {
            if (Mana.baseVal + Mana.diff >= 0) return Mana.baseVal + Mana.diff;
            else return 0;
        }
        public int GetMaxMana () {
            if (MaxMana.baseVal + MaxMana.diff >= 0) return MaxMana.baseVal + MaxMana.diff;
            else return 0;
        }
        public int GetManaRegen () {
            if (ManaRegen.baseVal + ManaRegen.diff >= 0) return ManaRegen.baseVal + ManaRegen.diff;
            else return 0;
        }
        public int GetShield () {
            if (Shield.baseVal + Shield.diff >= 0) return Shield.baseVal + Shield.diff;
            else return 0;
        }
        public int GetMaxShield () {
            if (MaxShield.baseVal + MaxShield.diff >= 0) return MaxShield.baseVal + MaxShield.diff;
            else return 0;
        }
        public int GetShieldRegen () {
            if (ShieldRegen.baseVal + ShieldRegen.diff >= 0) return ShieldRegen.baseVal + ShieldRegen.diff;
            else return 0;
        }
        public int GetStealthiness () {
            return Stealthiness.baseVal + Stealthiness.diff;
        }
        public int GetMaxStealthiness () {
            if (MaxStealthiness.baseVal + MaxStealthiness.diff >= 0) return MaxStealthiness.baseVal + MaxStealthiness.diff;
            else return 0;
        }
        public int GetStealthRegen () {
            if (StealthRegen.baseVal + StealthRegen.diff >= 0) return StealthRegen.baseVal + StealthRegen.diff;
            else return 0;
        }
        public int GetAtk () {
            if (Atk.baseVal + Atk.diff >= 0) return Atk.baseVal + Atk.diff;
            else return 0;
        }
        public int GetMp () {
            if (Mp.baseVal + Mp.diff >= 0) return Mp.baseVal + Mp.diff;
            else return 0;
        }
        public int GetDef () {
            if (Def.baseVal + Def.diff >= 0) return Def.baseVal + Def.diff;
            else return 0;
        }
        public int GetMR () {
            if (MR.baseVal + MR.diff >= 0) return MR.baseVal + MR.diff;
            else return 0;
        }
        public int GetSpeed () {
            if (Speed.baseVal + Speed.diff >= 0) return Speed.baseVal + Speed.diff;
            else return 0;
        }
        public int GetCalculation () {
            if (Calculation.baseVal + Calculation.diff >= 0) return Calculation.baseVal + Calculation.diff;
            else return 0;
        }

        // 0>=: 見えない, -calc <  < 0: 位置のみ, -calc <: 
        public void UpdateVisibility () {
            if (GetStealthiness () >= 0) {
                Id.visibility = false;
                Name.visibility = false;
                Position.visibility = false;
                Hp.visibility = false;
                MaxHp.visibility = false;
                HpRegen.visibility = false;
                Shield.visibility = false;
                MaxShield.visibility = false;
                ShieldRegen.visibility = false;
                Mana.visibility = false;
                MaxMana.visibility = false;
                ManaRegen.visibility = false;
                Stealthiness.visibility = false;
                MaxStealthiness.visibility = false;
                StealthRegen.visibility = false;
                Atk.visibility = false;
                Mp.visibility = false;
                Def.visibility = false;
                MR.visibility = false;
                Speed.visibility = false;
                Calculation.visibility = false;

            } else if (GetStealthiness () < 0) {
                if (GetStealthiness () < -GetCalculation ()) {
                    Id.visibility = true;
                    Name.visibility = true;
                    Position.visibility = true;
                    Hp.visibility = true;
                    MaxHp.visibility = true;
                    HpRegen.visibility = true;
                    Shield.visibility = true;
                    MaxShield.visibility = true;
                    ShieldRegen.visibility = true;
                    Mana.visibility = true;
                    MaxMana.visibility = true;
                    ManaRegen.visibility = true;
                    Stealthiness.visibility = true;
                    MaxStealthiness.visibility = true;
                    StealthRegen.visibility = true;
                    Atk.visibility = true;
                    Mp.visibility = true;
                    Def.visibility = true;
                    MR.visibility = true;
                    Speed.visibility = true;
                    Calculation.visibility = true;
                } else {
                    Id.visibility = true;
                    Name.visibility = true;
                    Position.visibility = true;
                    Hp.visibility = false;
                    MaxHp.visibility = false;
                    HpRegen.visibility = false;
                    Shield.visibility = false;
                    MaxShield.visibility = false;
                    ShieldRegen.visibility = false;
                    Mana.visibility = false;
                    MaxMana.visibility = false;
                    ManaRegen.visibility = false;
                    Stealthiness.visibility = false;
                    MaxStealthiness.visibility = false;
                    StealthRegen.visibility = false;
                    Atk.visibility = false;
                    Mp.visibility = false;
                    Def.visibility = false;
                    MR.visibility = false;
                    Speed.visibility = false;
                    Calculation.visibility = false;
                }
            }
        }

        public void SetPosition(int x, int y){
            if(x >= 0 && x < Board.BOARDSIZE && y >= 0 && y < Board.BOARDSIZE){
                Position.x = x;
                Position.y = y;
            } else{
                Debug.LogAssertion("Invalid input");
            }
        }

        public void SetHp (int value) {
            if (value >= GetMaxHp ()) {
                Hp.baseVal = GetMaxHp ();
            } else if (value <= 0) {
                Hp.baseVal = 0;
            } else {
                Hp.baseVal = value;
            }
        }

        public void SetShield (int value) {
            if (value >= GetMaxShield ()) {
                Shield.baseVal = GetMaxShield ();
            } else if (value <= 0) {
                Shield.baseVal = 0;
            } else {
                Shield.baseVal = value;
            }
        }
        public void SetMana (int value) {
            if (value >= GetMaxMana ()) {
                Shield.baseVal = GetMaxMana ();
            } else if (value <= 0) {
                Mana.baseVal = 0;
            } else {
                Mana.baseVal = value;
            }
        }

        // シリアライズ可能なClientUnit型を生成して返す
        public ClientUnit ConvertToClientUnit (bool applyVisibility) {
            ClientUnit instance = new ClientUnit ();
            if (!(applyVisibility) || Id.visibility) instance.Id = Id.value;
            if (!(applyVisibility) || Name.visibility) instance.Name = Name.value;
            instance.LocatedSide = LocatedSide;
            if (!(applyVisibility) || Position.visibility) instance.PositionX = Position.x;
            if (!(applyVisibility) || Position.visibility) instance.PositionY = Position.y;
            if (!(applyVisibility) || Hp.visibility) instance.Hp = GetHp ();
            if (!(applyVisibility) || MaxHp.visibility) instance.MaxHp = GetMaxHp ();
            if (!(applyVisibility) || HpRegen.visibility) instance.HpRegen = GetHpRegen ();
            if (!(applyVisibility) || Shield.visibility) instance.Shield = GetShield ();
            if (!(applyVisibility) || MaxShield.visibility) instance.MaxShield = GetMaxShield ();
            if (!(applyVisibility) || ShieldRegen.visibility) instance.ShieldRegen = GetShieldRegen ();
            if (!(applyVisibility) || Mana.visibility) instance.Mana = GetMana ();
            if (!(applyVisibility) || MaxMana.visibility) instance.MaxMana = GetMaxMana ();
            if (!(applyVisibility) || ManaRegen.visibility) instance.ManaRegen = GetManaRegen ();
            if (!(applyVisibility) || Stealthiness.visibility) instance.Stealthiness = GetStealthiness ();
            if (!(applyVisibility) || MaxStealthiness.visibility) instance.MaxStealthiness = GetMaxStealthiness ();
            if (!(applyVisibility) || StealthRegen.visibility) instance.StealthRegen = GetStealthRegen ();
            if (!(applyVisibility) || Atk.visibility) instance.Atk = GetAtk ();
            if (!(applyVisibility) || Mp.visibility) instance.Mp = GetMp ();
            if (!(applyVisibility) || Def.visibility) instance.Def = GetDef ();
            if (!(applyVisibility) || MR.visibility) instance.MR = GetMR ();
            if (!(applyVisibility) || Speed.visibility) instance.Speed = GetSpeed ();
            if (!(applyVisibility) || Calculation.visibility) instance.Calculation = GetCalculation ();
            instance.ActionPoint = ActionPoint;
            instance.Owner = Owner;
            instance.SkillList = new int[SkillList.Count];
            for (int i = 0; i < SkillList.Count; i++) {
                instance.SkillList[i] = SkillList[i].Id;
            }
            // BuffList
            instance.BuffTypeList = new BuffType[BuffList.Count];
            for (int i = 0; i < BuffList.Count; i++) {
                instance.BuffTypeList[i] = BuffList[i].Type;
            }
            instance.BuffPowerList = new int[BuffList.Count];
            for (int i = 0; i < BuffList.Count; i++) {
                instance.BuffPowerList[i] = BuffList[i].Power;
            }
            instance.BuffDurationList = new int[BuffList.Count];
            for (int i = 0; i < BuffList.Count; i++) {
                instance.BuffDurationList[i] = BuffList[i].Duration;
            }
            instance.DemonList = new int[DemonList.Count];
            for (int i = 0; i < DemonList.Count; i++) {
                instance.DemonList[i] = DemonList[i];
            }
            instance.UnitType = UnitType;
            return instance;
        }

        // データを読み込んでUnitを生成
        public Unit (int id, Players locatedSide, int x, int y, Players owner) {
            try {
                string filePath = @"Assets/Scripts/Server/DataBase/UnitData.csv";
                var fs = new FileStream (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader (fs, Encoding.GetEncoding ("Shift_JIS"));
                int count = 0;
                while (reader.Peek () >= 0) {
                    if (count == id) {
                        string[] cols = reader.ReadLine ().Split (',');
                        Id = (id, false);
                        Name = (cols[1], false); // 0はid
                        LocatedSide = locatedSide;
                        Position = (x, y, false);
                        Hp = (int.Parse (cols[2]), 0, false);
                        MaxHp = (int.Parse (cols[3]), 0, false);
                        HpRegen = (int.Parse (cols[4]), 0, false);
                        Shield = (int.Parse (cols[5]), 0, false);
                        MaxShield = (int.Parse (cols[6]), 0, false);
                        ShieldRegen = (int.Parse (cols[7]), 0, false);
                        Mana = (int.Parse (cols[8]), 0, false);
                        MaxMana = (int.Parse (cols[9]), 0, false);
                        ManaRegen = (int.Parse (cols[10]), 0, false);
                        Stealthiness = (int.Parse (cols[11]), 0, false);
                        MaxStealthiness = (int.Parse (cols[12]), 0, false);
                        StealthRegen = (int.Parse (cols[13]), 0, false);
                        Atk = (int.Parse (cols[14]), 0, false);
                        Mp = (int.Parse (cols[15]), 0, false);
                        Def = (int.Parse (cols[16]), 0, false);
                        MR = (int.Parse (cols[17]), 0, false);
                        Speed = (int.Parse (cols[18]), 0, false);
                        Calculation = (int.Parse (cols[19]), 0, false);
                        UnitType = (UnitType) Enum.Parse (typeof (UnitType), cols[20]);
                        ActionPoint = 0;
                        IsActive = true;
                        Owner = owner;
                        int skillnum = int.Parse (cols[21]);
                        SkillList = new List<Skill> ();
                        if (skillnum != 0) {
                            for (int i = 1; i <= skillnum; i++) {
                                SkillList.Add (new Skill (int.Parse (cols[21 + i])));
                            }
                        }
                        BuffList = new List<Buff> ();
                        int buffnum = int.Parse (cols[21 + skillnum + 1]);
                        if (buffnum != 0) {
                            for (int i = 1; i <= skillnum; i++) {
                                BuffList.Add (new Buff ((BuffType) Enum.Parse (typeof (BuffType), cols[21 + skillnum + 1 + i]), 0, Buff.INF));
                            }
                        }
                        // TODO: DemonList
                        DemonList = new List<int> ();

                        break;
                    }
                    reader.ReadLine ();
                    count++;
                }
                reader.Close ();
            } catch {
                Debug.LogAssertion("Unit.cs初期化エラー");
            }
        }

    }
}