using System;
using GameControllers.Entities.Enemies;
using GameControllers.Factories.Properties;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameControllers.Levels
{
    public class LevelLoader
    {
        private readonly LevelConfig[] _levelConfigs;
        private readonly ICanGetPoolEntity<Enemy> _enemiesPoolObjectsFactory;
        private readonly Transform[] _spawnPoints;
        private readonly Action<int, bool> _endWave;
        
        private float _currentLevelDurations;
        private float _currentIntervalSpawn;
        private int _currentLevelIndex;
        private LevelConfig _currentLevelConfig;

        public LevelLoader(
            LevelConfig[] levelConfigs,
            ICanGetPoolEntity<Enemy> enemiesPoolObjectsFactory,
            Transform[] spawnPoints,
            Action<int, bool> endWave)
        {
            _levelConfigs = levelConfigs;
            _enemiesPoolObjectsFactory = enemiesPoolObjectsFactory;
            _spawnPoints = spawnPoints;
            _endWave = endWave;

            SetLevelConfig();
        }
        
        public void Update()
        {
            _currentLevelDurations += Time.deltaTime;
            _currentIntervalSpawn += Time.deltaTime;

            if (_currentIntervalSpawn >= _currentLevelConfig.IntervalSpawn)
            {
                _currentIntervalSpawn = 0;

                var randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                
                var randomIndexConfig = Random.Range(0, _currentLevelConfig.EnemyConfigs.Length);
                
                _enemiesPoolObjectsFactory.GetPoolEntity(
                    randomSpawnPoint.position, 
                    randomSpawnPoint.rotation, 
                    _currentLevelConfig.EnemyConfigs[randomIndexConfig].Index);
            }

            if (_currentLevelDurations >= _currentLevelConfig.LevelDurations)
            {
                LoadNextLevel();
            }
        }

        private void SetLevelConfig()
        {
            _currentLevelConfig = _levelConfigs[_currentLevelIndex];
            _currentIntervalSpawn = _currentLevelConfig.IntervalSpawn;
            _currentLevelDurations = 0;
        }

        private void LoadNextLevel()
        {
            var canLoadNextLevel = _currentLevelIndex + 1 < _levelConfigs.Length;
            
            if (canLoadNextLevel)
                _currentLevelIndex++;
            
            _endWave.Invoke(_currentLevelIndex, canLoadNextLevel);
            SetLevelConfig();
        }
    }
}
