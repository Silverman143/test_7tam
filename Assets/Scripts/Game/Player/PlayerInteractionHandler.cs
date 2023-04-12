using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

namespace com.Test_7tam
{
    public class PlayerInteractionHandler : MonoBehaviourPun
    {
        public UnityEvent<float> OnGetHit = new UnityEvent<float>();
        public UnityEvent<CollectableObjectMono> OnCollected = new UnityEvent<CollectableObjectMono>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerDamager damager))
            {
                if (photonView.IsMine)
                {
                    OnGetHit?.Invoke(damager.DamageForce);
                    damager.Interact(DamagerInteractionType.player);
                }
            }
            else if (collision.TryGetComponent<CollectableObjectMono>(out CollectableObjectMono obj))
            {
                if (photonView.IsMine)
                {
                    OnCollected.Invoke(obj);
                    obj.Interact();
                }
            }
        }
    }
}
