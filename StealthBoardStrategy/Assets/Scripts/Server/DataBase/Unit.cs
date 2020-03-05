using System.Collections.Generic;

namespace StealthBoardStrategy.Server.DataBase{
    public abstract class Unit {
        public (int value, bool visibility) Id{get;set;}
        protected (string value, bool visibility) Name{get;set;}
        public (int x, int y, bool visibility) Position{get;set;}
        protected bool IsActive;
        public (int baseVal, int diff, bool visibility) Hp{get;set;}
        public (int baseVal, int diff, bool visibility) MaxHp{get;set;}
        public (int baseVal, int diff, bool visibility) HpRegen{get;set;}
        public (int baseVal, int diff, bool visibility) Shield{get;set;}
        public (int baseVal, int diff, bool visibility) MaxShield{get;set;}
        public (int baseVal, int diff, bool visibility) ShieldRegen{get;set;}
        public (int baseVal, int diff, bool visibility) Mana{get;set;}
        public (int baseVal, int diff, bool visibility) MaxMana{get;set;}
        public (int baseVal, int diff, bool visibility) ManaRegen{get;set;}
        public (int baseVal, int diff, bool visibility) Stealthiness{get;set;}
        public (int baseVal, int diff, bool visibility) StealthRegen{get;set;}
        public (int baseVal, int diff, bool visibility) MaxStealthiness{get;set;}
        public (int baseVal, int diff, bool visibility) Atk{get;set;}
        public (int baseVal, int diff, bool visibility) Mp{get;set;}
        public (int baseVal, int diff, bool visibility) Def{get;set;}
        public (int baseVal, int diff, bool visibility) MR{get;set;}
        public (int baseVal, int diff, bool visibility) Speed{get;set;}
        public (int baseVal, int diff, bool visibility) Calculation{get;set;}
        public int ActionPoint{get;set;}
        public List<Skill> SkillList;
        public List<Unit> DeamonList;
        public UnitType UnitType;
        
        public (int value, bool visibility) PrimaryDemon{get;set;}

        public int GetHp(){
            if(Hp.baseVal + Hp.diff >= 0) return Hp.baseVal + Hp.diff;
            else return 0;
        }
        public int GetMaxHp(){
            if(MaxHp.baseVal + MaxHp.diff >= 0) return MaxHp.baseVal + Hp.diff;
            else return 0;
        }
        public int GetHpRegen(){
            if(HpRegen.baseVal + HpRegen.diff >= 0) return HpRegen.baseVal + HpRegen.diff;
            else return 0;
        }
        public int GetMana(){
            if(Mana.baseVal + Mana.diff >= 0) return Mana.baseVal + Mana.diff;
            else return 0;
        }
        public int GetMaxMana(){
            if(MaxMana.baseVal + MaxMana.diff >= 0) return MaxMana.baseVal + MaxMana.diff;
            else return 0;
        }
        public int GetManaRegen(){
            if(ManaRegen.baseVal + ManaRegen.diff >= 0) return ManaRegen.baseVal + ManaRegen.diff;
            else return 0;
        }
        public int GetShield(){
            if(Shield.baseVal + Shield.diff >= 0) return Shield.baseVal + Shield.diff;
            else return 0;
        }
        public int GetMaxShield(){
            if(MaxShield.baseVal + MaxShield.diff >= 0) return MaxShield.baseVal + MaxShield.diff;
            else return 0;
        }
        public int GetShieldRegen(){
            if(ShieldRegen.baseVal + ShieldRegen.diff >= 0) return ShieldRegen.baseVal + ShieldRegen.diff;
            else return 0;
        }
        public int GetStealthiness(){
            if(Stealthiness.baseVal + Stealthiness.diff >= 0) return Stealthiness.baseVal + Stealthiness.diff;
            else return 0;
        }
        public int GetStealthRegen(){
            if(StealthRegen.baseVal + StealthRegen.diff >= 0) return StealthRegen.baseVal + StealthRegen.diff;
            else return 0;
        }
        public int GetMaxStealthiness(){
            if(MaxStealthiness.baseVal + MaxStealthiness.diff >= 0) return MaxStealthiness.baseVal + MaxStealthiness.diff;
            else return 0;
        }
        public int GetAtk(){
            if(Atk.baseVal + Atk.diff >= 0) return Atk.baseVal + Atk.diff;
            else return 0;
        }
        public int GetMp(){
            if(Mp.baseVal + Mp.diff >= 0) return Mp.baseVal + Mp.diff;
            else return 0;
        }
        public int GetDef(){
            if(Def.baseVal + Def.diff >= 0) return Def.baseVal + Def.diff;
            else return 0;
        }
        public int GetMR(){
            if(MR.baseVal + MR.diff >= 0) return MR.baseVal + MR.diff;
            else return 0;
        }
        public int GetSpeed(){
            if(Speed.baseVal + Speed.diff >= 0) return Speed.baseVal + Speed.diff;
            else return 0;
        }
        public int GetCalculation(){
            if(Calculation.baseVal + Calculation.diff >= 0) return Calculation.baseVal + Calculation.diff;
            else return 0;
        }

        public void UpdateVisibility(){

        }
    }
}