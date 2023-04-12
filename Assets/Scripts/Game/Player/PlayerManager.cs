using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using System;

namespace com.Test_7tam
{
    [RequireComponent(typeof(HealthHandler))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerInteractionHandler))]
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject[] _playerSpritesObjects;
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;
        [SerializeField] private float _shootingSpeed = 1;

        public static GameObject LocalPlayerInstance;

        private HealthHandler _healthHandler;
        private PlayerMovementControllerMono _moveController;
        private PlayerInteractionHandler _interactionHandler;

        private float _shootingTimer = 0;

        #region Events

        public UnityEvent<float> OnGetDamage = new UnityEvent<float>();
        public UnityEvent<float> OnGetHealing = new UnityEvent<float>();
        
        public UnityEvent<Vector2> OnShoot = new UnityEvent<Vector2>();
        public UnityEvent OnPlayerDead = new UnityEvent();

        public static UnityEvent<GameObject> OnLocalPlayerChanged = new UnityEvent<GameObject>();

        #endregion

        private void Awake()
        {
            FindComponents();
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
                OnLocalPlayerChanged.Invoke(LocalPlayerInstance);
                ShootButton.Instance.OnShootButtonDown.AddListener(Shoot);
            }
        }

        public override void OnEnable()
        {
            if (!photonView.IsMine)
                return;
            base.OnEnable();
            _interactionHandler.OnGetHit.AddListener(GetHit);
            _interactionHandler.OnCollected.AddListener(CollectItem);
            _healthHandler.OnHealthEnded.AddListener(KillPlayer);
        }

        public override void OnDisable()
        {
            if (!photonView.IsMine)
                return;

            base.OnDisable();
            _interactionHandler.OnGetHit.RemoveListener(GetHit);
            _interactionHandler.OnCollected.RemoveListener(CollectItem);
            ShootButton.Instance.OnShootButtonDown.RemoveListener(Shoot);
            _healthHandler.OnHealthEnded.RemoveListener(KillPlayer);
        }

        private void FixedUpdate()
        {
            if (_shootingTimer >= 0)
            {
                _shootingTimer -= Time.deltaTime;
            }
        }

        public void ActivateSprite(int index)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("ActivateSpriteRPC", RpcTarget.All, index);
            }
        }

        [PunRPC]
        public void ActivateSpriteRPC(int index)
        {
            GameObject spriteObject = _playerSpritesObjects[index];
            spriteObject.SetActive(true);
            _playerSpriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
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

        private void CollectItem(CollectableObjectMono item)
        {
            if (!photonView.IsMine)
                return;
            if(item is Coin)
            {
                PlayerDataHandler.Instance.AddCoins();
            }
            else
            {
                Debug.LogError("The player collected an unknown object");
            }
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
            if (!photonView.IsMine)
                return;

            if (_shootingTimer <= 0)
            {
                OnShoot.Invoke(_moveController.LookVector.normalized);
                _shootingTimer = _shootingSpeed;
            }
            
        }

        IEnumerator GetHitEffect()
        {
            _playerSpriteRenderer.color = Color.red;

            yield return new WaitForSeconds(1);

            _playerSpriteRenderer.color = Color.white;
        }
    }
}
