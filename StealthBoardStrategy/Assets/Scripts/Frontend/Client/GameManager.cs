using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StealthBoardStrategy.Frontend.Client {
    public class GameManager : MonoBehaviourPunCallbacks {
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