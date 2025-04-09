using System.Collections.Generic;
using GameControllers.Factories.PoolObjectsFactories;
using GameControllers.FSM.Properties;
using UnityEngine;

namespace GameControllers.FSM
{
    public abstract class LevelSpawnerState : IState
    {
        private readonly float _levelDurations;
        private readonly float _intervalSpawn;
        private readonly int _indexLevel;
        private readonly EnemiesPoolObjectsFactory _enemiesPoolObjectsFactory;
        private readonly Transform[] _spawnPoints;
        private readonly Dictionary<int, List<int>> _enemiesConfigs;
        
        private float _currentLevelDurations;
        private float _currentIntervalSpawn;
        
        public LevelSpawnerState(
            float levelDurations, 
            float intervalSpawn, 
            int indexLevel,
            EnemiesPoolObjectsFactory enemiesPoolObjectsFactory, 
            Transform[] spawnPoints,
            Dictionary<int, List<int>> enemiesConfigs)
        {
            _levelDurations = levelDurations;
            _intervalSpawn = intervalSpawn;
            _indexLevel = indexLevel;
            _enemiesPoolObjectsFactory = enemiesPoolObjectsFactory;
            _spawnPoints = spawnPoints;
            _enemiesConfigs = enemiesConfigs;
        }
        
        public void Enter()
        {
            _currentIntervalSpawn = _intervalSpawn;
        }

        public void Exit()
        {
            _currentIntervalSpawn = 0;
            _currentLevelDurations = 0;
        }

        public void Update()
        {
            _currentLevelDurations += Time.deltaTime;
            _currentIntervalSpawn += Time.deltaTime;

            if (_currentIntervalSpawn >= _intervalSpawn)
            {
                _currentIntervalSpawn = 0;

                var randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                
                var randomIndexConfig = _enemiesConfigs[_indexLevel]
                    [Random.Range(0, _enemiesConfigs[_indexLevel].Count)];
                
                _enemiesPoolObjectsFactory.GetPoolEntity(
                    randomSpawnPoint.position, randomSpawnPoint.rotation, randomIndexConfig);
            }

            if (_currentLevelDurations >= _levelDurations)
            {
                if (!CanLoadNextState())
                {
                    _currentLevelDurations = 0;
                }
            }
        }
        
        protected abstract bool CanLoadNextState();
    }
}
