using System.Collections.Generic;
using GameControllers.Entities.Enemies.Configs;
using GameControllers.Factories.PoolObjectsFactories;
using UnityEngine;

namespace GameControllers.FSM.MultiPlayer.States
{
    public class SecondMultiPlayerState : LevelSpawnerState
    {
        private readonly StateMachine _stateMachine;
        
        private const float LevelDurations = 40f;
        private const float IntervalSpawn = 6f;
        private const int IndexLevel = 1;
        
        public SecondMultiPlayerState(
            StateMachine stateMachine,
            EnemiesPoolObjectsFactory enemiesPoolObjectsFactory,
            Transform[] spawnPoints, 
            Dictionary<int, List<int>> enemiesConfigs) 
            : base(
                LevelDurations,
                IntervalSpawn, 
                IndexLevel,
                enemiesPoolObjectsFactory,
                spawnPoints,
                enemiesConfigs)
        {
            _stateMachine = stateMachine;
        }

        protected override bool CanLoadNextState()
        {
            return false;
        }
    }
}
