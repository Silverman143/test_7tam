using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace com.Test_7tam
{
    public class PlayerFollower : MonoBehaviourPun
    {
        [SerializeField] private Transform _characterTransform;
        [Range(0, 1)]
        [SerializeField] private float _smoothnessSpeed;
        [SerializeField] private bool _distant;
        [SerializeField] private float _distantVolume;
        [SerializeField] private Vector3 _offset;


        // Update is called once per frame

        private void FixedUpdate()
        {

            if (_characterTransform == null)
            {
                _characterTransform = PlayerManager.LocalPlayerInstance.transform;
            }
            else
            {
                Follow();
            }
        }

        private void Follow()
        {
            Vector3 targetPos = _characterTransform.position + _offset;
            if (_distant)
            {
                targetPos += _offset * _distantVolume;
            }
            Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, _smoothnessSpeed);
            smoothPos.z = 0;
            transform.position = smoothPos;
        }

        public void SetDistance(bool isDistant)
        {
            if (isDistant)
            {
                _distant = true;
            }
            else
            {
                _distant = false;
            }
        }
    }
}
