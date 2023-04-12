using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

namespace com.Test_7tam
{
    public class PlayerDataHandler : MonoBehaviourPun
    {
        [SerializeField] private float _maxHealth;
        public float MaxHealth => _maxHealth;
        public int _coins;

        public static PlayerDataHandler Instance;

        public UnityEvent<int> OnCoinsAmountChanged = new UnityEvent<int>();

        private void Awake()
        {
            if (!photonView.IsMine)
                return;

            if (Instance != this)
            {
                Instance = this;
            }
        }


        public void AddCoins(int amount = 1)
        {
            if (!photonView.IsMine)
                return;

            _coins += amount;
            OnCoinsAmountChanged.Invoke(_coins);
        }

        public void RemoveCoin(int amount = 1)
        {
            if (!photonView.IsMine)
                return;

            _coins -= amount;
            OnCoinsAmountChanged.Invoke(_coins);
        }
    }
}
