using System;
using System.Collections.Generic;
using GameControllers.Factories.PoolObjectsFactories;
using GameControllers.FSM.Properties;
using GameControllers.FSM.SinglePlayer.States;
using GameControllers.Levels;
using UnityEngine;

namespace GameControllers.FSM.SinglePlayer
{
    public class SinglePlayerStateMachine : StateMachine
    {
        public SinglePlayerStateMachine(
            EnemiesPoolObjectsFactory enemiesPoolObjectsFactory, 
            Transform[] spawnPoints,
            Dictionary<int, List<int>> enemiesConfigs,
            LevelConfig[] levelConfigs)
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(FirstSinglePlayerState)] = 
                    new FirstSinglePlayerState(
                        this, enemiesPoolObjectsFactory, spawnPoints, enemiesConfigs),
                [typeof(SecondSinglePlayerState)] =
                    new SecondSinglePlayerState(
                        this, enemiesPoolObjectsFactory, spawnPoints, enemiesConfigs)
            };
            
            EnterIn<FirstSinglePlayerState>();
        }
    }
}
