using System.Collections.Generic;
using GameControllers.Entities.Enemies.Configs;
using GameControllers.Factories.PoolObjectsFactories;
using UnityEngine;

namespace GameControllers.FSM.MultiPlayer.States
{
    public class FirstMultiPlayerState : LevelSpawnerState
    {
        private readonly StateMachine _stateMachine;
        
        private const float LevelDurations = 60f;
        private const float IntervalSpawn = 10f;
        private const int IndexLevel = 0;
        
        public FirstMultiPlayerState(
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
            _stateMachine.EnterIn<SecondMultiPlayerState>();
            return true;
        }
    }
}
