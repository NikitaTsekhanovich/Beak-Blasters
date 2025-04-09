using GameControllers.AttackEntities.Types;
using GameControllers.Entities.Bonuses;
using GameControllers.Factories.Properties;
using GameControllers.GameLogic;
using GameControllers.GameStartControllers.ControllerInitializationData;
using GameControllers.Spawners;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;
using Zenject;

namespace GameControllers.GameStartControllers
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private SpawnerPlayersData _spawnerPlayersData;
        [SerializeField] private SpawnerEnemiesData _spawnerEnemiesData;
        
        [Inject] private DiContainer _container;
        [Inject] private ICanGetPoolEntity<Bullet> _bulletFactory;
        [Inject] private ICanGetPoolEntity<BonusPower> _bonusFactory;
        [Inject] private ICanGetPoolEntity<Grenade> _grenadeFactory;
        [Inject] private ICanGetPoolEntity<Rocket> _rocketFactory;
        
        private SpawnerEnemies _spawnerEnemies;
        
        private void Start()
        {
            SetNetworkSettings();
            CreatePlayerSpawner();
            StartAttackEntityFactories();
            CreateBonusesFactory();
            CreateEnemiesSpawner();
        }

        private void Update()
        {
            if (GameModeData.ModeGame == ModeGame.Single || PhotonNetwork.IsMasterClient)
                _spawnerEnemies.Update();
        }

        private void SetNetworkSettings()
        {
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                PhotonNetwork.SendRate = 20;
                PhotonNetwork.SerializationRate = 15;
            }
        }

        private void CreatePlayerSpawner()
        { 
            new SpawnerPlayers(_spawnerPlayersData, _container);
        }
        
        private void StartAttackEntityFactories()
        {
            _bulletFactory.StartFactory();
            _grenadeFactory.StartFactory();
            _rocketFactory.StartFactory();
        }

        private void CreateBonusesFactory()
        {
            if (GameModeData.ModeGame == ModeGame.Single || PhotonNetwork.IsMasterClient)
                _bonusFactory.StartFactory();
        }
 
        private void CreateEnemiesSpawner()
        {
            _spawnerEnemies = new SpawnerEnemies(_spawnerEnemiesData, _container);
        }
    }
}
