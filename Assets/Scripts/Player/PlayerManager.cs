using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

namespace com.Test_7tam
{
    [RequireComponent(typeof(HealthHandler))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerInteractionHandler))]
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;

        public static GameObject LocalPlayerInstance;

        private HealthHandler _healthHandler;
        private PlayerMovementControllerMono _moveController;
        private PlayerInteractionHandler _interactionHandler;

        #region Events

        public UnityEvent<float> OnGetDamage = new UnityEvent<float>();
        public UnityEvent<float> OnGetHealing = new UnityEvent<float>();
        
        public UnityEvent<Vector2> OnShoot = new UnityEvent<Vector2>();
        public UnityEvent OnPlayerDead = new UnityEvent();

        #endregion

        private void Awake()
        {
            FindComponents();
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                Debug.LogError($"Setting local Player !!!!!!!!!!!");
                PlayerManager.LocalPlayerInstance = this.gameObject;
                ShootButton.Instance.OnShootButtonDown.AddListener(Shoot);
            }
        }

        public override void OnEnable()
        {
            if (!photonView.IsMine)
                return;
            base.OnEnable();
            _interactionHandler.OnGetHit.AddListener(GetHit);
        }

        public override void OnDisable()
        {
            if (!photonView.IsMine)
                return;

            base.OnDisable();
            _interactionHandler.OnGetHit.RemoveListener(GetHit);
            ShootButton.Instance.OnShootButtonDown.RemoveListener(Shoot);
        }

        private void FindComponents()
        {
            if (!photonView.IsMine)
                return;

            _healthHandler = GetComponent<HealthHandler>();
            _moveController = GetComponent<PlayerMovementControllerMono>();
            _interactionHandler = GetComponent<PlayerInteractionHandler>();
        }

        public void GetHit(float value)
        {
            if (!photonView.IsMine)
                return;

            Debug.Log("Palyer got hit");
            OnGetDamage.Invoke(value);
            StartCoroutine(GetHitEffect());
        }

        public void GetHealing(float value)
        {
            if (!photonView.IsMine)
                return;

            OnGetHealing.Invoke(value);
        }

        private void Activate()
        {
            if (!photonView.IsMine)
                return;

            _healthHandler.enabled = true;
            _moveController.enabled = true;
        }

        private void Deactivate()
        {
            if (!photonView.IsMine)
                return;
            _healthHandler.enabled = false;
            _moveController.enabled = false;

        }

        private void CollectItem()
        {
            if (!photonView.IsMine)
                return;

        }

        private void KillPlayer()
        {
            if (!photonView.IsMine)
                return;

            Deactivate();
            OnPlayerDead.Invoke();
        }

        private void Shoot()
        {
            OnShoot.Invoke(_moveController.MoveVector.normalized);
        }

        IEnumerator GetHitEffect()
        {
            _playerSpriteRenderer.color = Color.red;

            yield return new WaitForSeconds(1);

            _playerSpriteRenderer.color = Color.white;
        }
    }
}
