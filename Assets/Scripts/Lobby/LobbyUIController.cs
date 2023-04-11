using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Test_7tam
{
    public class LobbyUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _userNamePanel;
        [SerializeField] private GameObject _connectionErrorPanel;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _loadingPanel;

        private void Start()
        {
            ShowUsername();
        }

        private void OnEnable()
        {
            LobbyEvents.OnStartConnecting.Add(ShowLoading);
            LobbyEvents.OnConnError.Add(ShowConnectionError);
            LobbyEvents.OnConnectionComplete.Add(ShowMenu);
            LobbyEvents.OnLoadingStart.Add(ShowLoading);
            LobbyEvents.OnLoadingEnd.Add(HideLoading);
        }

        private void OnDisable()
        {
            LobbyEvents.OnStartConnecting.Remove(ShowLoading);
            LobbyEvents.OnConnError.Remove(ShowConnectionError);
            LobbyEvents.OnConnectionComplete.Remove(ShowMenu);
            LobbyEvents.OnLoadingStart.Remove(ShowLoading);
            LobbyEvents.OnLoadingEnd.Remove(HideLoading);
        }

        private void ShowMenu()
        {
            _connectionErrorPanel.SetActive(false);
            _userNamePanel.SetActive(false);
            _loadingPanel.SetActive(false);
            _menuPanel.SetActive(true);
        }

        private void ShowUsername()
        {
            _connectionErrorPanel.SetActive(false);
            _loadingPanel.SetActive(false);
            _menuPanel.SetActive(false);
            _userNamePanel.SetActive(true);
        }

        private void ShowConnectionError()
        {
            _loadingPanel.SetActive(false);
            _menuPanel.SetActive(false);
            _userNamePanel.SetActive(false);
            _connectionErrorPanel.SetActive(true);
        }

        private void ShowLoading()
        {
            _loadingPanel.SetActive(true);
        }

        private void HideLoading()
        {
            _loadingPanel.SetActive(false);
        }

    }
}


