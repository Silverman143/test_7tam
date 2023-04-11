using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace com.Test_7tam
{
    public class JoinRoomButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _input;

        public void JoinRoom()
        {
            if(_input.text != string.Empty)
            {
                LobbyEvents.OnJoinRoomButton.Publish(_input.text);
            }
            else
            {
                Debug.LogError("Join room name is Empty");
            }
            
        }
    }
}
