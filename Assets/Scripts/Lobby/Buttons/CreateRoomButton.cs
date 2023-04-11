using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace com.Test_7tam
{
    public class CreateRoomButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputName;

        public void CreateRoom()
        {
            if(_inputName.text != string.Empty)
            {
                LobbyEvents.OnCreateRoomButton.Publish(_inputName.text);
            }
        }
    }
}
