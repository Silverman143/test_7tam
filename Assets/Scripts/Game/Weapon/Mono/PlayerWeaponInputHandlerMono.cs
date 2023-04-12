using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
namespace com.Test_7tam
{
    public class PlayerWeaponInputHandlerMono : MonoBehaviourPunCallbacks
    {
        [SerializeField] private protected float _damageForce;
        [SerializeField] private protected float _rechargeSpeed;

        [SerializeField] private protected Transform _attackLine;

        private protected float _timer = 0;
        private protected bool _isActive = false;
        private protected Vector2 _attackVector;

        public UnityEvent<Vector2> OnShoot = new UnityEvent<Vector2>();

        public override void OnEnable()
        {
            base.OnEnable();
            if (!photonView.IsMine)
                return;

            ShootFixedJoystick.Instance.OnStart.AddListener(StartAiming);
            ShootFixedJoystick.Instance.OnStop.AddListener(Attack);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (!photonView)
                return;

            ShootFixedJoystick.Instance.OnStart.RemoveListener(StartAiming);
            ShootFixedJoystick.Instance.OnStop.RemoveListener(Attack);
        }

        private protected virtual void Update()
        {
            if (!photonView)
                return;

            Aiming();
        }

        private protected virtual void StartAiming()
        {
            Debug.Log("Start");
            _attackLine.gameObject.SetActive(true);
            _isActive = true;
        }

        private protected virtual void Aiming()
        {
            if (!_isActive)
                return;

            _attackVector = new Vector2(ShootFixedJoystick.Instance.Horizontal, ShootFixedJoystick.Instance.Vertical);
            float angle = Mathf.Atan2(_attackVector.y, _attackVector.x) * Mathf.Rad2Deg;
            _attackLine.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private protected virtual void Attack()
        {
            if (!_isActive)
                return;

            _isActive = false;
            _attackLine.gameObject.SetActive(false);
            OnShoot.Invoke(_attackVector);
        }
    }
}
