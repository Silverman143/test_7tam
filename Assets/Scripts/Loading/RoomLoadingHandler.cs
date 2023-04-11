using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace com.Test_7tam
{
    public class RoomLoadingHandler : MonoBehaviourPunCallbacks
    {
        private int _maxUsers;

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            _maxUsers = PhotonNetwork.CurrentRoom.MaxPlayers;
            PhotonNetwork.AutomaticallySyncScene = true;
            LoadingEvents.OnRoomLoaded.Publish(PhotonNetwork.CurrentRoom.Name, _maxUsers);
            LoadingEvents.OnUserConnected.Publish(PhotonNetwork.LocalPlayer.NickName, PhotonNetwork.LocalPlayer.UserId);
        }

        #region Private Methods

        private void CheckPlayersAmount()
        {
            if(_maxUsers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Debug.Log("Room is full!!!!!!!!");
                StartCoroutine(LoadGameCoroutine());
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            LoadingEvents.OnUserConnected.Publish(other.NickName, other.UserId);
            if (PhotonNetwork.IsMasterClient)
            {
                CheckPlayersAmount();
            }
        }

        public override void OnJoinedRoom()
        {
            LoadingEvents.OnRoomLoaded.Publish(PhotonNetwork.CurrentRoom.Name, _maxUsers);
            Player[] players = PhotonNetwork.PlayerList;
            foreach(Player player in players)
            {
                LoadingEvents.OnUserConnected.Publish(player.NickName, player.UserId);
            }
        }

        #endregion

        private IEnumerator LoadGameCoroutine()
        {
            yield return new WaitForSeconds(3);
            PhotonNetwork.LoadLevel(2);
        }
    }
}


