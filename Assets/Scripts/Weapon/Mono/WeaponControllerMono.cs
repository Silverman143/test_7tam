using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.UIElements.Experimental;

namespace com.Test_7tam
{
    public class WeaponControllerMono : PlayerDamager
    {
        [SerializeField] private protected Vector2 _moveVector;
        [SerializeField] private protected Transform _sprite;
        [SerializeField] private protected float _moveSpeed;
        [SerializeField] private protected float _lifeTime;
        [SerializeField] private protected bool _isActivate = false;


        private protected virtual void FixedUpdate()
        {
            if (_isActivate)
            {
                Move();
                Rotate();
            }
        }

        public void Activate(Vector2 moveVector, Collider2D playerCollider)
        {
            SetCollisionIgnore(playerCollider);
            SetMoveVector(moveVector);
            _isActivate = true;
        }

        private void SetCollisionIgnore(Collider2D playerCollider)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
        }

        private void SetMoveVector(Vector2 vector)
        {
            _moveVector = vector.normalized;
        }

        private protected virtual void Move()
        {
            transform.Translate(_moveVector * _moveSpeed * Time.deltaTime);
        }

        private protected virtual void Rotate()
        {
            if (_moveVector != Vector2.zero)
            {
                float angle = Mathf.Atan2(_moveVector.y, _moveVector.x) * Mathf.Rad2Deg;
                _sprite.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        public override void Interact(DamagerInteractionType type)
        {
            switch (type)
            {
                case DamagerInteractionType.player:
                    _isActivate = false;

                    if (photonView.IsMine)
                    {
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                    else
                    {
                        photonView.RPC("DestroyObj", photonView.Owner);
                    }
                    break;

                case DamagerInteractionType.other:
                    _isActivate = false;
                    break;
            }
        }

        [PunRPC]
        public void DestroyObj()
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerManager>(out PlayerManager player))
            {
                return;
            }
            else
            {
                Interact(DamagerInteractionType.other);
            }
        }
    }
}
