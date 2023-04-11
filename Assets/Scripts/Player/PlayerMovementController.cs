using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

namespace com.Test_7tam
{
    public class PlayerMovementController : PlayerMovementControllerMono
    {
        private Vector3 _defaultSpriteScale;
        private bool _walkAnimationActive = false;

        protected override void Awake()
        {
            _defaultSpriteScale = _playerSprite.localScale;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            Animation();
        }

        private void Animation()
        {
            if(_moveVector!= Vector2.zero && !_walkAnimationActive)
            {
                _walkAnimationActive = true;
                StartCoroutine(WalkAnimationCoroutine());
            }
            else if(_moveVector == Vector2.zero && _walkAnimationActive)
            {
                _walkAnimationActive = false;
                StopCoroutine(WalkAnimationCoroutine());
            }
        }

        IEnumerator WalkAnimationCoroutine()
        {
            if (_walkAnimationActive)
            {
                float speed = 0.3f;

                if (_playerSprite.localScale.x < 1 * _defaultSpriteScale.x)
                {
                    _playerSprite.DOScale(1.05f * _defaultSpriteScale.x, speed);
                }
                else
                {
                    _playerSprite.DOScale(0.95f * _defaultSpriteScale.x, speed);
                }

                yield return new WaitForSeconds(speed);
                StartCoroutine(WalkAnimationCoroutine());
            }
        }
    }

}
