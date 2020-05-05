using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StealthBoardStrategy.Frontend.Client {
    public class GameManager : MonoBehaviourPunCallbacks {
        [SerializeField]
        private string playerPrefabName;

        public override void OnLeftRoom () {
            SceneManager.LoadScene (0);
        }

        public override void OnPlayerEnteredRoom (Photon.Realtime.Player other) {
            Debug.LogFormat ("OnPlayerEnteredRoom() {0}", other.NickName);
            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat ("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena ();
            }
        }

        public override void OnPlayerLeftRoom (Photon.Realtime.Player other) {
            Debug.LogFormat ("OnPlayerLeftRoom() {0}", other.NickName);
            if (PhotonNetwork.IsMasterClient) {
                Debug.LogFormat ("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
                LoadArena ();
            }
        }

        private void Start () {
            if (ClientBattleManager.LocalPlayerInstance == null) {
                Debug.LogFormat ("We are Instatiating LocalPlayer from {0}", Application.loadedLevelName);
                PhotonNetwork.Instantiate (this.playerPrefabName, new Vector3 (0, 0, 0), Quaternion.identity);
            }else{
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        public void LeaveRoom () {
            PhotonNetwork.LeaveRoom ();
        }

        private void LoadArena () {
            if (!PhotonNetwork.IsMasterClient) {
                Debug.LogError ("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat ("PhotonNetwork : Loading");
            PhotonNetwork.LoadLevel ("Room");
        }
    }
}