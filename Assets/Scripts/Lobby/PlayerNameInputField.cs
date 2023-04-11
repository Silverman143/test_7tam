using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace com.Test_7tam
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        // Store the PlayerPref Key to avoid typos
        const string playerNamePrefKey = "PlayerName";

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {

            string defaultName = "player"+Random.Range(0, 1000);
            TMP_InputField _inputField = GetComponent<TMP_InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        // Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        public void SetPlayerName(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
            Debug.Log($"<color=green>Set new player name = {value}</color>");
        }

        #endregion
    }
}


