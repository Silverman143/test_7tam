using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;

namespace com.Test_7tam
{
    public class HealthHandler : MonoBehaviourPunCallbacks, IPunObservable
    {
        private float _maxHealth;
        private float _healthWas;
        [SerializeField] private float _currentHealth;
        public float CurrentHealth => _currentHealth;

        public UnityEvent<float> OnHealthChanged = new UnityEvent<float>();
        public UnityEvent OnHealthEnded = new UnityEvent();

        [SerializeField] private PlayerManager _playerManager;



        private void Awake()
        {
            if (!photonView.IsMine)
                return;
            _playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            if (!photonView.IsMine)
                return;
            _currentHealth = PlayerDataHandler.Instance.MaxHealth;
            _maxHealth = PlayerDataHandler.Instance.MaxHealth;
        }

        private void FixedUpdate()
        {
            if (_healthWas != _currentHealth)
            {
                OnHealthChanged.Invoke(_currentHealth);
                _healthWas = _currentHealth;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!photonView.IsMine)
                return;

            _playerManager.OnGetDamage.AddListener(GetDamage);
            _playerManager.OnGetHealing.AddListener(GetHealing);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (!photonView.IsMine)
                return;

            _playerManager.OnGetDamage.RemoveListener(GetDamage);
            _playerManager.OnGetHealing.RemoveListener(GetHealing);
        }

        private void GetDamage(float value)
        {
            if (!photonView.IsMine)
                return;

            _currentHealth -= value;
            OnHealthChanged.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                OnHealthEnded.Invoke();
            }
        }

        private void GetHealing(float value)
        {
            if (!photonView.IsMine)
                return;

            _currentHealth += value;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }


            OnHealthChanged.Invoke(_currentHealth);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(_currentHealth);
            }
            else
            {
                // Network player, receive data
                this._currentHealth = (float)stream.ReceiveNext();
            }
        }
    }
}
