using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Test_7tam
{
    [System.Serializable]
    public enum DamagerInteractionType
    {
        player, other
    }

    public class PlayerDamager : MonoBehaviourPun
    {
        [SerializeField] private protected float _damageForce;
        public float DamageForce => _damageForce;

        public virtual void Interact(DamagerInteractionType type)
        {

        }
    }
}
