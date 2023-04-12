using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace com.Test_7tam
{
    public class LeaveRoomButton : MonoBehaviour
    {
        public void LeaveRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            PhotonNetwork.LoadLevel(0);
        }
    }
}
