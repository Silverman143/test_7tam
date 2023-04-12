using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

namespace com.Test_7tam
{
    public class CollectableObjectMono : MonoBehaviourPun
    {
        public virtual void Interact()
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                photonView.RPC("DestroyThis", photonView.Owner);
            }
        }

        [PunRPC]
        public void DestroyThis()
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
            else
            {
                photonView.RPC("DestroyThis", photonView.Owner);
            }
        }
    }
}
