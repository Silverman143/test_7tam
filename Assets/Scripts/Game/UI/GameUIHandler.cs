using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

namespace com.Test_7tam
{
    public class GameUIHandler : MonoBehaviourPun
    {
        [SerializeField] private GameObject _joystick;
        [SerializeField] private GameObject _shootButton;

        [SerializeField] private GameObject _finishGamePanel;
        [SerializeField] private GameObject _winGameObj;
        [SerializeField] private GameObject _gameOverObj;

        [SerializeField] private TextMeshProUGUI _positionTMP;

        [SerializeField] private GameObject _coinsGameCounter;
        [SerializeField] private GameObject _coinsGameFinishTMP;

        public static GameUIHandler Instance;

        private PlayerDataHandler _playerDataHandler;

        private TextMeshProUGUI _coinsCounter;

        public void Awake()
        {
            if (Instance != this)
            {
                Instance = this;
            }

            PlayerManager.OnLocalPlayerChanged.AddListener(StartListenLocalPlayer);
        }

        private void Start()
        {
            _coinsCounter = _coinsGameCounter.GetComponentInChildren<TextMeshProUGUI>();
            UpdateCoinsAmount(0);
        }

        private void StartListenLocalPlayer(GameObject player)
        {
            _playerDataHandler?.OnCoinsAmountChanged.RemoveListener(UpdateCoinsAmount);

            _playerDataHandler = player.GetComponent<PlayerDataHandler>();
            _playerDataHandler.OnCoinsAmountChanged.AddListener(UpdateCoinsAmount);

        }

        private void OnEnable()
        {
            _playerDataHandler?.OnCoinsAmountChanged.AddListener(UpdateCoinsAmount);
        }

        private void OnDisable()
        {
            _playerDataHandler?.OnCoinsAmountChanged.RemoveListener(UpdateCoinsAmount);
        }

        public void ShowEndGamePanel(int place)
        {
            _joystick.SetActive(false);
            _shootButton.SetActive(false);
            _coinsGameCounter.SetActive(false);
            _finishGamePanel.SetActive(true);

            _coinsCounter = _coinsGameFinishTMP.GetComponent<TextMeshProUGUI>();
            UpdateCoinsAmount(PlayerDataHandler.Instance._coins);

            if (place == 1)
            {
                _winGameObj.SetActive(true);
                _gameOverObj.SetActive(false);

            }
            else
            {
                _winGameObj.SetActive(false);
                _gameOverObj.SetActive(true);
            }

            _positionTMP.text = $"{place} th";
        }

        private void UpdateCoinsAmount(int amount)
        {
            _coinsCounter.text = amount.ToString();
        }
    }
}
