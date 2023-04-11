using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace com.Test_7tam
{
    [System.Serializable]
    public struct CardData
    {
        public string ID;
        public UserCardHandlerMono Card;
    }

    public class LoadingMenuHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _roomName;

        [SerializeField] private TextMeshProUGUI _usersCounterTMP;
        [SerializeField] private Transform _userCardsPanel;
        [SerializeField] private GameObject _userCardPrefab;

        private List<CardData> _usersCards;

        private int _maxUsers;

        private void Awake()
        {
            _usersCards = new List<CardData>();
        }

        private void OnEnable()
        {
            LoadingEvents.OnRoomLoaded.Add(UploadRoomData);
            LoadingEvents.OnUserConnected.Add(AddUser);
            LoadingEvents.OnUserDisconnected.Add(RemoveUser);
        }

        private void OnDisable()
        {
            LoadingEvents.OnRoomLoaded.Remove(UploadRoomData);
            LoadingEvents.OnUserConnected.Remove(AddUser);
            LoadingEvents.OnUserDisconnected.Remove(RemoveUser);
        }

        private void UploadRoomData(string roomName, int maxUsers)
        {
            _maxUsers = maxUsers;
            _roomName.text = roomName;
            _usersCounterTMP.text = $"1/{_maxUsers}";

        }

        private void UpdateUsersAmountInRoom()
        {
            int usersAmount = _usersCards.Count;
            _usersCounterTMP.text = $"{usersAmount}/{_maxUsers}";
        }

        private void AddUser(string name, string id)
        {
            GameObject newCard = Instantiate(_userCardPrefab, _userCardsPanel);
            LoadingUserCard card = newCard.GetComponent<LoadingUserCard>();
            card.SetName(name);
            CardData data = new CardData { Card = card, ID = id };
            _usersCards.Add(data);
            UpdateUsersAmountInRoom();
        }

        private void RemoveUser(string id)
        {
            foreach (CardData card in _usersCards)
            {
                if (string.Equals(card.ID, id))
                {
                    Destroy(card.Card.gameObject);
                    _usersCards.Remove(card);
                }
            }
        }

        #region RPC Methods



        #endregion
    }
}


