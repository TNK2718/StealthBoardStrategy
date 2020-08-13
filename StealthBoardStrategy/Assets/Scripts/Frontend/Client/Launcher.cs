using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace StealthBoardStrategy.Frontend.Client {
    // Launcher.
    public class Launcher : MonoBehaviourPunCallbacks {
        string gameVersion = "1";
        bool IsConnecting;
        [SerializeField]
        private byte maxPlayersPerRoom = 2;
        [SerializeField]
        private GameObject controlPanel;
        [SerializeField]
        private GameObject progressLabel;

        void Awake () {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start () {
            progressLabel.SetActive (false);
            controlPanel.SetActive (true);
        }

        public void Connect () {
            IsConnecting = true;
            progressLabel.SetActive (true);
            controlPanel.SetActive (false);
            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.JoinRandomRoom ();
            } else {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings ();
            }
        }

        public override void OnConnectedToMaster () {
            Debug.Log ("OnConnectedMaster() was called by PUN");
            if (IsConnecting) {
                PhotonNetwork.JoinRandomRoom ();
                IsConnecting = false;
            }
        }

        public override void OnDisconnected (DisconnectCause cause) {
            progressLabel.SetActive (false);
            controlPanel.SetActive (true);
            Debug.LogWarningFormat ("OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed (short returnCode, string message) {
            Debug.Log ("OnJoinRandomFailed() was called by PUN. No random room available, so we create one ./nCalling: PhtonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom (null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom () {
            Debug.Log ("OnJoinedRoom() was called by PUN. Now this is in a room");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
                Debug.Log ("We load the 'Room'");
                PhotonNetwork.LoadLevel ("Room");
            }
        }
    }
}