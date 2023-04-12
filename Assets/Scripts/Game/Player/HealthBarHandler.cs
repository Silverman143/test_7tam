using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace com.Test_7tam
{
    public class HealthBarHandler : MonoBehaviourPun
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private HealthHandler _healthHandler;

        private float _playerMaxHealth;

        private void Start()
        {
            _playerMaxHealth = PlayerDataHandler.Instance.MaxHealth;
        }

        private void OnEnable()
        {
            _healthHandler.OnHealthChanged.AddListener(UpdateData);
        }

        private void OnDisable()
        {
            _healthHandler.OnHealthChanged.RemoveListener(UpdateData);
        }

        private void UpdateData(float currentHealth)
        {
            float value = currentHealth / _playerMaxHealth;
            _slider.value = value;
        }
    }
}
