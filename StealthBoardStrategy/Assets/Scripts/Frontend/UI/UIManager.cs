using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using StealthBoardStrategy.Frontend.Events;
using StealthBoardStrategy.Frontend.Client;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.Events;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace StealthBoardStrategy.Frontend.UI {
    public class UIManager : MonoBehaviour {
        public ClientBattleManager clientBattleManager;

        // Start is called before the first frame update
        void Start () {

        }

        // Update is called once per frame
        void FixedUpdate () {

        }

        public void TurnEndEventToClient(){
            clientBattleManager.EndActionPhase();
        }
    }
}