using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

namespace com.Test_7tam
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        #endregion

        #region Private Fields

        //client's version number
        string gameVersion = "1";

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        #endregion

        public override void OnEnable()
        {
            base.OnEnable();
            LobbyEvents.OnCreateRoomButton.Add(CreateRoom);
            LobbyEvents.OnJoinRoomButton.Add(ConnectRoom);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            LobbyEvents.OnCreateRoomButton.Remove(CreateRoom);
            LobbyEvents.OnJoinRoomButton.Remove(ConnectRoom);
        }

        #region Public Methods

        public void Connect()
        {
            
            if (!PhotonNetwork.IsConnected)
            {
                LobbyEvents.OnStartConnecting.Publish();
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void CreateRoom(string roomName)
        {
            if (PhotonNetwork.IsConnected)
            {
                LobbyEvents.OnLoadingStart.Publish();

                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = (byte)RoomSizeSelector.Instance.RoomSize;
                PhotonNetwork.CreateRoom(roomName, roomOptions);
            }
            else
            {
                Debug.LogError("Lost connection");
                LobbyEvents.OnLoadingEnd.Publish();
            }
        }

        public void ConnectRoom(string name)
        {
            if (PhotonNetwork.IsConnected)
            {
                LobbyEvents.OnLoadingStart.Publish();
                PhotonNetwork.JoinRoom(name);
            }
            else
            {
                Debug.LogError("Lost connection");
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            Debug.Log($"<color=red>ID = {PhotonNetwork.LocalPlayer.ActorNumber} </color>");
            LobbyEvents.OnConnectionComplete.Publish();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            LobbyEvents.OnConnError.Publish();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogError("Can't connect room");

            LobbyEvents.OnLoadingEnd.Publish();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("<color=red>join room </color>");
        }


        public override void OnCreatedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }

        #endregion
    }
}

