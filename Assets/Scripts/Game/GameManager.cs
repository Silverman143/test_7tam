using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

namespace com.Test_7tam
{

    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _weaponPref;

        [SerializeField] Transform[] _spawnPoints;
        [SerializeField] private List<Player> _deadPlayers;

        private void Awake()
        {
            _playerPrefab = Resources.Load<GameObject>("PlayerPrefab");
            _weaponPref = Resources.Load<GameObject>("Knife");
        }
        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (_playerPrefab == null)
                {
                    Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
                }
                else
                {
                    Player[] players = PhotonNetwork.PlayerList;
                    Debug.Log($"players = {players.Length}");

                    for(int i=0; i<players.Length; i++)
                    {
                        photonView.RPC("CreatePlayer", players[i], i);
                    }
                }
            }

           
        }

        [PunRPC]
        private void CreatePlayer(int index)
        {
            GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoints[index].position, Quaternion.identity, 0);
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            playerManager.OnShoot.AddListener(CreateKnife);
            playerManager.OnPlayerDead.AddListener(PlayerDead);
        }

        private void PlayerDead()
        {
            photonView.RPC("PlayerDeadMaster", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer);
        }

        [PunRPC]
        private void PlayerDeadMaster(Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            _deadPlayers.Add(player);

        }

        [PunRPC]
        private void AddDeadPlayer(Player player)
        {

        }

        public override void OnEnable()
        {
            base.OnEnable();

        }

        public override void OnDisable()
        {
            base.OnDisable();
            PlayerManager.LocalPlayerInstance.GetComponent<PlayerWeaponInputHandlerMono>().OnShoot.RemoveListener(CreateKnife);

        }

        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        private void CheckPlayersAmount()
        {
            Player[] players = PhotonNetwork.PlayerList;

            if (players.Length < 2)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(0);
            }
        }

        private void DestroyGameObject(GameObject obj)
        {
            PhotonNetwork.Destroy(obj);
        }

        private void CreateKnife(Vector2 moveVector)
        {
            GameObject newKnife = PhotonNetwork.Instantiate(_weaponPref.name, PlayerManager.LocalPlayerInstance.transform.position, Quaternion.identity);
            WeaponControllerMono weapon = newKnife.GetComponent<WeaponControllerMono>();
            weapon.Activate(moveVector, PlayerManager.LocalPlayerInstance.GetComponent<Collider2D>());
        }

        #endregion

        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
            CheckPlayersAmount();
        }

        #endregion
    }
}


