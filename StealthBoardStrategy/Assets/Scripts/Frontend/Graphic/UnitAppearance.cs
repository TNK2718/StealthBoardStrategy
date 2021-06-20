using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using StealthBoardStrategy.Frontend.Events;
using StealthBoardStrategy.Frontend.UI;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace StealthBoardStrategy.Frontend.Graphic
{
    public class UnitAppearance : MonoBehaviour
    {
        public int Id;
        public (int x, int y) Position;
        public (int current, int max) Hp;
        SpriteRenderer spriteRenderer;


        public void Initialize(int _id, (int x, int y) _position, (int current, int max) _hp){
            Id = _id;
            Position = _position;
            Hp = _hp;
            Sprite image = Resources.Load<Sprite>("Images/unit/unit" + Id.ToString());

            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = image;
        }

    }
}