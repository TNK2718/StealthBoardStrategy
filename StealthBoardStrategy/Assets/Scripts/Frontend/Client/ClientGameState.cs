using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using StealthBoardStrategy.Server.DataBase;
using StealthBoardStrategy.Server.GameLogic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StealthBoardStrategy.Frontend.Client {
    public enum ClientGameState {
        None, Matching, WaitingForInput, WaitingForResponse
    }
}