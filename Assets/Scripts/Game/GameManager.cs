using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Linq;

namespace com.Test_7tam
{

    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _weaponPref;
        [SerializeField] private GameObject _coinPrefab;

        [SerializeField] private Transform _groundTransform;

        [SerializeField] Transform[] _spawnPoints;
        [SerializeField] private List<Player> _deadPlayers;

        private List<Player> _activePlayers;

        private void Awake()
        {
            _playerPrefab = Resources.Load<GameObject>("PlayerPrefab");
            _weaponPref = Resources.Load<GameObject>("Knife");
            _deadPlayers = new List<Player>();
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
                    CreateCoins();

                    Player[] players = PhotonNetwork.PlayerList;

                    for(int i=0; i<players.Length; i++)
                    {
                        photonView.RPC("CreatePlayer", players[i], i);
                    }
                    _activePlayers = players.ToList<Player>();
                }
            }

           
        }

        [PunRPC]
        private void CreatePlayer(int index)
        {
            GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoints[index].position, Quaternion.identity, 0);
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            playerManager.ActivateSprite(index);
            playerManager.OnShoot.AddListener(CreateKnife);
            playerManager.OnPlayerDead.AddListener(PlayerDead);
        }

        private void CreateCoins()
        {
            int amount = Random.Range(10, 20);

            Vector2[] points = MathMethods.GetRandom2DPointsOnGroundSprite(_groundTransform, amount, 10);

            for(int i = 0; i<amount; i++)
            {
                Vector3 pos = points[i];
                pos.z = 3;
                PhotonNetwork.Instantiate(_coinPrefab.name, pos, Quaternion.identity, 0);
            }
        }

        private void PlayerDead()
        {
            PhotonNetwork.Destroy(PlayerManager.LocalPlayerInstance);
            photonView.RPC("PlayerDeadMaster", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer);
        }

        [PunRPC]
        private void PlayerDeadMaster(Player player)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            AddDeadPlayer(player);
            int position = _activePlayers.Count + 1;
            photonView.RPC("ShowEndGamePanel", player, position);
            if(_activePlayers.Count == 1)
            {
                photonView.RPC("ShowEndGamePanel", _activePlayers[0], 1);
            }
        }

        [PunRPC]
        private void ShowEndGamePanel(int position)
        {
            Debug.Log("show eng game RPC works");
            GameUIHandler.Instance.ShowEndGamePanel(position);
        }

        private void AddDeadPlayer(Player player)
        {

            if (!_deadPlayers.Contains(player))
            {
                _activePlayers.Remove(player);
                _deadPlayers.Add(player);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

        }

        public override void OnDisable()
        {
            base.OnDisable();
            PlayerManager.LocalPlayerInstance?.GetComponent<PlayerWeaponInputHandlerMono>().OnShoot.RemoveListener(CreateKnife);

        }

        #region Photon Callbacks

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
                //PhotonNetwork.LeaveRoom();
                //PhotonNetwork.LoadLevel(0);


            }
        }

        [PunRPC]
        private void DestroyGameObject(GameObject obj)
        {
            PhotonView objView = obj.GetPhotonView();
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(obj);
            }
            else
            {
                photonView.RPC("DestroyGameObject", PhotonNetwork.MasterClient, obj);
            }
            
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
                AddDeadPlayer(other);
            }
            CheckPlayersAmount();
        }

        #endregion
    }
}


