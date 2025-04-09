using GameControllers.Entities.Enemies;
using GameControllers.Factories.PoolObjectsFactories;
using GameControllers.Factories.Properties;
using GameControllers.GameStartControllers.ControllerInitializationData;
using GameControllers.Levels;
using GameControllers.PlayerControllers;
using Photon.Pun;
using StartSceneControllers;
using Zenject;

namespace GameControllers.Spawners
{
    public class SpawnerEnemies
    {
        private readonly LevelLoader _levelLoader;

        public SpawnerEnemies(SpawnerEnemiesData spawnerEnemiesData, DiContainer container)
        {
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;
            
            var enemyPrefab = spawnerEnemiesData.EnemyPrefab;

            ICanGetPoolEntity<Enemy> enemiesFactory = new EnemiesPoolObjectsFactory(enemyPrefab, container);
            enemiesFactory.StartFactory();
            
            var player = container.Resolve<Player>();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                _levelLoader = new LevelLoader(
                    spawnerEnemiesData.NetworkLevelsConfigs,
                    enemiesFactory, 
                    spawnerEnemiesData.SpawnPoints,
                    player.UIWaveController.ShowWaveText);
            else if (GameModeData.ModeGame == ModeGame.Single)
                _levelLoader = new LevelLoader(
                    spawnerEnemiesData.LocalLevelsConfigs,
                    enemiesFactory, 
                    spawnerEnemiesData.SpawnPoints,
                    player.UIWaveController.ShowWaveText);
        }
        
        public void Update()
        {
            _levelLoader.Update();
        }
    }
}

