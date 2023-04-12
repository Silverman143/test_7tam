using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Test_7tam
{
    public class PlayerMovementControllerMono : MonoBehaviourPun
    {
        [SerializeField] protected Transform _playerSprite;

        [SerializeField] protected float _speed;
        [SerializeField] protected Vector2 _moveVector;
        [SerializeField] protected Vector2 _lookVector;
        public Vector2 MoveVector => _moveVector;
        public Vector2 LookVector => _lookVector;

        protected virtual void Awake()
        {
            _moveVector = Vector2.zero;
        }

        protected virtual void FixedUpdate()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            Move();
            Rotate();
        }

        protected virtual void Move()
        {
            _moveVector = new Vector2(MoveFixedJoystick.Instance.Horizontal, MoveFixedJoystick.Instance.Vertical);
            transform.Translate(_moveVector * Time.deltaTime * _speed);
        }

        protected virtual void Rotate()
        {
            if (_moveVector != Vector2.zero)
            {
                float angle = Mathf.Atan2(_moveVector.y, _moveVector.x) * Mathf.Rad2Deg;
                _playerSprite.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                _lookVector = _moveVector;
            }
        }
    }
}