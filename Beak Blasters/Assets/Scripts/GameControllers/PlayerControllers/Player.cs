using Containers;
using GameControllers.Factories.Properties;
using GameControllers.GameLogic;
using GameControllers.PlayerControllers.PlayerCanvasControllers;
using GameControllers.Weapons;
using MusicSystem;
using Photon.Pun;
using SaveSystems;
using SceneSwitchHandlers;
using StartSceneControllers;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameControllers.PlayerControllers
{
    public class Player : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        [SerializeField] private AudioListener _audioListener;
        [Header("Player components")]
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private InterfaceController _interfaceController;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private CoinsController _coinsController;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private PhysicsMovement _physicsMovement;
        [SerializeField] private HealthPlayer _healthPlayer;
        [SerializeField] private UIWaveController _uiWaveController;
        [SerializeField] private BoostHandler _boostHandler;
        [Header("Player settings components")]
        [SerializeField] private Transform _weaponSpawnPoint;
        [Header("PhysicsMovement components")]
        [SerializeField] private FixedJoystick _joystick;
        [SerializeField] private Transform _skinTransform;
        [Header("TimerRespawn components")]
        [SerializeField] private TMP_Text _timerRespawnText;
        
        private TimerRespawn _timerRespawn;
        private Rigidbody2D _rigidbody;
        private SaveSystem _saveSystem;
        private StoreItemsConfigsContainer _storeItemsConfigsContainer;
        private ICanGetNewObject<Weapon> _weaponFactory;
        private GameStateController _gameStateController;
        private Weapon _weapon;
        private bool _isDead;
        
        public ScoreController ScoreController => _scoreController;
        public CoinsController CoinsController => _coinsController;
        public UIWaveController UIWaveController => _uiWaveController;

        [Inject]
        private void Construct(
            SceneDataLoader sceneDataLoader,
            MusicController musicController,
            SaveSystem saveSystem,
            ICanGetNewObject<Weapon> weaponFactory,
            StoreItemsConfigsContainer storeItemsConfigsContainer,
            GameStateController gameStateController,
            SoundsContainer soundsContainer)
        {
            _saveSystem = saveSystem;
            _storeItemsConfigsContainer = storeItemsConfigsContainer;
            _weaponFactory = weaponFactory;
            _gameStateController = gameStateController;
            
            _playerSettings.Initialize(saveSystem, storeItemsConfigsContainer);
            _interfaceController.Initialize(sceneDataLoader, musicController, saveSystem, soundsContainer);
            _loseScreen.Initialize(saveSystem, soundsContainer);
            _coinsController.Initialize(saveSystem);
            _scoreController.Initialize(saveSystem);
            _uiWaveController.Initialize(soundsContainer);
            
            if (photonView.IsMine || GameModeData.ModeGame == ModeGame.Single)
            {
                _playerSettings.SetSkins();
                SpawnWeapon();

                InitHealth();
                _healthPlayer.OnDeathLocalPlayer += Die;
                
                _timerRespawn = new TimerRespawn(_timerRespawnText, photonView, _gameStateController, _healthPlayer);
                _timerRespawn.OnRespawn += Respawn;
                
                _gameStateController.SubscribeToEvents(_healthPlayer, _timerRespawn);
            }

            if (GameModeData.ModeGame == ModeGame.Multiplayer && !photonView.IsMine)
            {
                _audioListener.enabled = false;
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            InitMovement();
        }
        
        private void Update()
        {
            if (GameModeData.ModeGame == ModeGame.Single && _isDead) return;
            
            if (photonView.IsMine || GameModeData.ModeGame == ModeGame.Single)
            {
                _physicsMovement.SetJoystickInfo(_joystick);
                _physicsMovement.DoRotate();
                _timerRespawn.PlayRespawnTimer();
                _timerRespawn.PlayTimer();
            }
            else
            {
                
            }
            
            _weapon?.UpdateSystem();
        }

        private void FixedUpdate()
        {
            if (GameModeData.ModeGame == ModeGame.Single && _isDead) return;
            
            if (photonView.IsMine || GameModeData.ModeGame == ModeGame.Single)
            {
                _physicsMovement.DoMove();
            }
            else
            {
                _physicsMovement.SmoothMovement();
            }

            _boostHandler.FixedUpdateSystem();
        }

        private void OnDestroy()
        {
            if (_timerRespawn != null)
            {
                _timerRespawn.Dispose();
                _timerRespawn.OnRespawn -= Respawn;
            }
            _healthPlayer.OnDeathLocalPlayer -= Die;
            HealthPlayer.OnSendDeathClonePlayer -= DieClone;
        }

        private void InitHealth()
        {
            _healthPlayer.Initialize(
                _weapon.GetComponent<SpriteRenderer>(), 
                _storeItemsConfigsContainer.SkinsConfigs[_playerSettings.IndexChosenIndex].GameSprite,
                _storeItemsConfigsContainer.SkinsConfigs[_playerSettings.IndexChosenIndex].StartHealth,
                _storeItemsConfigsContainer.SkinsConfigs[_playerSettings.IndexChosenIndex].MaximumHealth);
            
            HealthPlayer.OnSendDeathClonePlayer += DieClone;
        }

        private void Respawn()
        {
            _weapon.CanShoot(true);
            _isDead = false;
        }

        private void DieClone(string playerName)
        {
            if (photonView.IsMine)
                _healthPlayer.SendDeathClonePlayer(playerName);
        }

        private void Die(string playerName)
        {
            _weapon.CanShoot(false);
            _isDead = true;
            _rigidbody.linearVelocity = Vector2.zero;
        }

        private void InitMovement()
        {
            _physicsMovement.Init(_skinTransform, _rigidbody);
        }

        private void SpawnWeapon()
        {
            var weaponIndex = _saveSystem.GameSaveData.StoreWeaponsData.IndexChosenItem;
            
            _weapon = _weaponFactory.GetNewObject(_weaponSpawnPoint, weaponIndex);
            _weapon.transform.SetParent(_weaponSpawnPoint);
            _weapon.Initialize(gameObject.tag, photonView.ViewID);
            _weapon.CanShoot(true);

            if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                photonView.RPC("SendSetWeaponParent", RpcTarget.Others, 
                    _weapon.gameObject.GetPhotonView().ViewID);
            }
        }
        
        [PunRPC]
        private void SendSetWeaponParent(int idWeapon)
        {
            var childPhotonView = PhotonView.Find(idWeapon);
            _weapon = childPhotonView.GetComponent<Weapon>();
            childPhotonView.transform.SetParent(_weaponSpawnPoint);
            InitHealth();
        }

        [PunRPC]
        private void SpawnedClone()
        {
            _gameStateController.UpdatePlayerCount(GameModeData.PlayerCount);
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (!photonView.IsMine)
            {
                var sceneContext = FindObjectOfType<SceneContext>();

                if (sceneContext != null)
                {
                    sceneContext.Container.Inject(this);
                    photonView.RPC("SpawnedClone", RpcTarget.Others);
                }
            }
        }
    }
}
