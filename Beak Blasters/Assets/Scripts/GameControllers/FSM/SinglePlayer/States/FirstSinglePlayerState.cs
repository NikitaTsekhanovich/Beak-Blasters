using System.Collections.Generic;
using GameControllers.Entities.Enemies.Configs;
using GameControllers.Factories.PoolObjectsFactories;
using UnityEngine;

namespace GameControllers.FSM.SinglePlayer.States
{
    public class FirstSinglePlayerState : LevelSpawnerState
    {
        private readonly StateMachine _stateMachine;
        
        private const float LevelDurations = 30f;
        private const float IntervalSpawn = 10f;
        private const int IndexLevel = 0;
        
        public FirstSinglePlayerState(
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
            _stateMachine.EnterIn<SecondSinglePlayerState>();
            return true;
        }
    }
}
