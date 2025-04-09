using Containers;
using DG.Tweening;
using GameControllers.Entities.Bonuses;
using GameControllers.Entities.Properties;
using GameControllers.Factories.Properties;
using GameControllers.PlayerControllers;
using GameControllers.Weapons;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace GameControllers.Entities.Enemies
{
    public class Enemy : Entity<Enemy>, ICanTakeDamage, IPunObservable, IPunInstantiateMagicCallback
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _weaponSpawnPoint;
        [SerializeField] private Collider2D _enemyCollider;
        [SerializeField] private Image _healthBar;
        [SerializeField] private GameObject _healthBlock;
        [SerializeField] private Transform _targetMovement;
        [SerializeField] private AudioSource _takeDamageSound;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private ParticleSystem _deathParticles;

        [Inject] private ICanGetNewObject<Weapon> _weaponFactory;
        [Inject] private EnemiesConfigsContainer _enemiesConfigsContainer;
        [Inject] private Player _player;
        [Inject] private ICanGetPoolEntity<BonusPower> _bonusFactory;
        
        private const float DelayAppear = 1.6f;
        private const float FlickerSpriteTime = 0.2f;
        
        private PhysicsMovement _physicsMovement;
        private HealthEnemy _healthEnemy;
        private Weapon _currentWeapon;
        private float _speed;
        private bool _readyMove;
        private Vector3 _position;
        private Quaternion _rotation;
        private int _levelEnemy;
        private Sequence _spriteAnimation;
        private Sequence _appearAnimation;
        private int _attackerId;

        [Inject]
        private void Constructor()
        {
            CreateComponents();
        }
        
        private void Update()
        {
            if (!_readyMove) return;
            
            _currentWeapon?.UpdateSystem();
        }
        
        private void FixedUpdate()
        {
            if (!_readyMove) return;
            
            if (photonView.IsMine || GameModeData.ModeGame == ModeGame.Single)
            {
                _physicsMovement.FixedUpdate();
            }
            else
            {
                _physicsMovement.SynchronizeMovement(_position);
                _physicsMovement.SynchronizeRotate(_rotation);
            }
        }
        
        private void CreateComponents()
        {
            _healthEnemy = new HealthEnemy(
                ReturnToPool, 
                _healthBar, 
                _takeDamageSound, 
                _deathSound,
                _deathParticles);
            _physicsMovement = new PhysicsMovement(_targetMovement, transform);
        }

        public override void ActiveInit(Vector3 startPosition, Quaternion startRotation)
        {
            var randomAngleZ = Random.Range(0f, 360f);
            base.ActiveInit(startPosition, Quaternion.Euler(0, 0, randomAngleZ));
            
            Appear();
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("Appear", RpcTarget.Others);
        }
        
        protected override void ReturnToPool()
        {
            if (!_readyMove) return;
            _readyMove = false;
            
            if (GameModeData.ModeGame == ModeGame.Single)
            {
                Destroy(_currentWeapon.gameObject);
                var newBonus = (BonusPower)_bonusFactory.GetPoolEntity(transform.position, transform.rotation);
                newBonus.Initialize(_levelEnemy);
            }
            else if (GameModeData.ModeGame == ModeGame.Multiplayer && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(_currentWeapon.gameObject);
                var newBonus = (BonusPower)_bonusFactory.GetPoolEntity(transform.position, transform.rotation);
                newBonus.photonView.RPC("Initialize", RpcTarget.All, _levelEnemy);
            }
            
            _healthBlock.SetActive(false);
            _spriteRenderer.enabled = false;
            _enemyCollider.enabled = false;
            _player.ScoreController.IncreaseScore(_attackerId);
            _player.CoinsController.IncreaseCoins(_levelEnemy);
            _physicsMovement?.StopRotate();
            
            DOTween.Sequence()
                .AppendInterval(1.5f)
                .AppendCallback(base.ReturnToPool);
        }

        private void OnDestroy()
        {
            _spriteAnimation?.Kill();
            _appearAnimation?.Kill();
            _physicsMovement?.StopRotate();
        }

        private void OnAnimationSprites(SpriteRenderer spriteRenderer)
        {
            _spriteAnimation = DOTween.Sequence()
                .Append(spriteRenderer.DOColor(Color.gray, FlickerSpriteTime))
                .Append(spriteRenderer.DOColor(Color.white, FlickerSpriteTime))
                .SetLoops(4, LoopType.Yoyo);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("HorizontalWall"))
                _physicsMovement.ChangeDirection(360);
            else if (collision.gameObject.CompareTag("VerticalWall"))
                _physicsMovement.ChangeDirection(180);
        }

        private void SpawnWeapon(int indexEnemyConfig)
        {
            var availableIndexWeapon = Random.Range(0, 
                _enemiesConfigsContainer.EnemiesConfigs
                    [indexEnemyConfig].AvailableIndexesWeapons.Length);
            
            var indexWeapon = _enemiesConfigsContainer.EnemiesConfigs
                [indexEnemyConfig].AvailableIndexesWeapons[availableIndexWeapon];
            
            _currentWeapon = _weaponFactory.GetNewObject(
                _weaponSpawnPoint, 
                indexWeapon);
            _currentWeapon.transform.SetParent(_weaponSpawnPoint);
            _currentWeapon.Initialize(gameObject.tag);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                photonView.RPC("SendSetEnemyWeaponParent", RpcTarget.Others, 
                    _currentWeapon.gameObject.GetPhotonView().ViewID,
                    _weaponSpawnPoint.gameObject.GetPhotonView().ViewID,
                    transform.rotation);
            }
        }
        
        public void GetIndexConfig(int indexEnemyConfig)
        {
            SpawnWeapon(indexEnemyConfig);
            SetConfig(indexEnemyConfig);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("SetConfig", RpcTarget.Others, indexEnemyConfig);
        }
        
        public void TakeDamage(int damage, int ownerId)
        {
            SyncTakeDamage(damage, ownerId);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("SyncTakeDamage", RpcTarget.Others, damage, ownerId);
        }

        [PunRPC]
        private void SyncTakeDamage(int damage, int ownerId)
        {
            _attackerId = ownerId;
            _healthEnemy.TakeDamage(damage);
        }

        [PunRPC]
        private void Appear()
        {
            _spriteRenderer.enabled = true;
            _healthBlock.SetActive(true);
            
            _appearAnimation = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one, DelayAppear))
                .AppendCallback(() =>
                {
                    _readyMove = true;
                    _enemyCollider.enabled = true;
                    _currentWeapon.CanShoot(true);
                });

            OnAnimationSprites(_spriteRenderer);
        }
        
        [PunRPC]
        private void SendSetEnemyWeaponParent(int idWeapon, int idParent, Quaternion rotation)
        {
            var childPhotonView = PhotonView.Find(idWeapon);
            var parentPhotonView = PhotonView.Find(idParent);

            childPhotonView.transform.SetParent(parentPhotonView.transform);
            childPhotonView.transform.rotation = rotation;
        }

        [PunRPC]
        private void SetConfig(int indexEnemyConfig)
        {
            var config = _enemiesConfigsContainer.EnemiesConfigs[indexEnemyConfig];
            
            _spriteRenderer.sprite = config.Sprite;
            
            _levelEnemy = config.LevelEnemy;
            _healthEnemy.SetValues(config.MaxHealth);
            _physicsMovement.InitSpeed(config.Speed);
        }

        [PunRPC]
        private void SyncPosition(Vector3 startPosition, Quaternion startRotation)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }

        [PunRPC]
        private void SendReturnToPool()
        {
            _physicsMovement?.StopRotate();
        }
        
        [PunRPC]
        private void SyncStateEntity(bool state)
        {
            gameObject.SetActive(state);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else if (stream.IsReading)
            {
                _position = (Vector3)stream.ReceiveNext();
                _rotation = (Quaternion)stream.ReceiveNext();
            }
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (!photonView.IsMine)
            {
                var sceneContext = FindObjectOfType<SceneContext>();

                if (sceneContext != null)
                {
                    sceneContext.Container.Inject(this);
                }
            }
        }
    }
}

