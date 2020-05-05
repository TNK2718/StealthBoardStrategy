using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using StealthBoardStrategy.Server.GameLogic;
using StealthBoardStrategy.Server.DataBase;

namespace StealthBoardStrategy.Frontend.Client {
    public class ClientUnit {
        public int Id{get;set;}
        public string Name{get;set;}
        public Players LocatedSide{get;set;}
        public int PositionX{get;set;}
        public int PositionY{get;set;}
        protected bool IsActive;
        public int Hp{get;set;}
        public int MaxHp{get;set;}
        public int HpRegen{get;set;}
        public int Shield{get;set;}
        public int MaxShield{get;set;}
        public int ShieldRegen{get;set;}
        public int Mana{get;set;}
        public int MaxMana{get;set;}
        public int ManaRegen{get;set;}
        public int Stealthiness{get;set;}
        public int StealthRegen{get;set;}
        public int MaxStealthiness{get;set;}
        public int Atk{get;set;}
        public int Mp{get;set;}
        public int Def{get;set;}
        public int MR{get;set;}
        public int Speed{get;set;}
        public int Calculation{get;set;}
        public int ActionPoint{get;set;}
        public Players Owner{get; set;}
        public Skill[] SkillList;
        public Buff[] BuffList;
        public int[] DemonList;
        public UnitType UnitType;
    }
}