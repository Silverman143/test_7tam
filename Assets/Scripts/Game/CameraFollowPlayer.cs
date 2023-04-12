using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace com.Test_7tam
{
    public class CameraFollowPlayer : MonoBehaviourPun
    {
        [SerializeField] private Transform _characterTransform;
        [Range(0, 1)]
        [SerializeField] private float _smoothnessSpeed;
        [SerializeField] private bool _distant;
        [SerializeField] private float _distantVolume;
        [SerializeField] private Vector3 _offset;

        [SerializeField] Vector2 _maxRestrictions;
        [SerializeField] Vector2 _minRestrictions;


        private float _screenHalfWidth;
        private float _sceenHalfHeight;

        // Update is called once per frame

        private void Awake()
        {
            Camera mainCamera = Camera.main;
            _screenHalfWidth = Camera.main.orthographicSize * mainCamera.aspect;
            _sceenHalfHeight = Camera.main.orthographicSize;

            _maxRestrictions.x = _maxRestrictions.x - _screenHalfWidth;
            _maxRestrictions.y = _maxRestrictions.y - _sceenHalfHeight;

            _minRestrictions.x = _minRestrictions.x + _screenHalfWidth;
            _minRestrictions.y = _minRestrictions.y + _sceenHalfHeight;
        }

        private void FixedUpdate()
        {

            if (_characterTransform == null)
            {
                if (PlayerManager.LocalPlayerInstance)
                {
                    _characterTransform = PlayerManager.LocalPlayerInstance.transform;
                }
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

            targetPos.x = Mathf.Clamp(targetPos.x, _minRestrictions.x, _maxRestrictions.x);
            targetPos.y = Mathf.Clamp(targetPos.y, _minRestrictions.y, _maxRestrictions.y);

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
