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
    [Serializable]
    public class ClientUnit {
        public bool Visibility;
        public int? Id;
        public string Name;
        public Players LocatedSide;
        public int? PositionX;
        public int? PositionY;
        protected bool IsActive;
        public int? Hp;
        public int? MaxHp;
        public int? HpRegen;
        public int? Shield;
        public int? MaxShield;
        public int? ShieldRegen;
        public int? Mana;
        public int? MaxMana;
        public int? ManaRegen;
        public int? Stealthiness;
        public int? StealthRegen;
        public int? MaxStealthiness;
        public int? Atk;
        public int? Mp;
        public int? Def;
        public int? MR;
        public int? Speed;
        public int? Calculation;
        public int? Agility;
        public int? ActionPoint;
        public Players Owner;
        public int[] SkillList;
        public BuffType[] BuffTypeList;
        public int[] BuffPowerList;
        public int[] BuffDurationList;
        public int[] DemonList;
        public UnitType UnitType;
    }
}