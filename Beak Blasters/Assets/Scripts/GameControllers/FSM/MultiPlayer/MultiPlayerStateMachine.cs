using System;
using System.Collections.Generic;
using GameControllers.Entities.Enemies.Configs;
using GameControllers.Factories.PoolObjectsFactories;
using GameControllers.FSM.MultiPlayer.States;
using GameControllers.FSM.Properties;
using GameControllers.Levels;
using UnityEngine;

namespace GameControllers.FSM.MultiPlayer
{
    public class MultiPlayerStateMachine : StateMachine
    {
        public MultiPlayerStateMachine(
            EnemiesPoolObjectsFactory enemiesPoolObjectsFactory,
            Transform[] spawnPoints,
            Dictionary<int, List<int>> enemiesConfigs,
            LevelConfig[] levelConfigs)
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(FirstMultiPlayerState)] = 
                    new FirstMultiPlayerState(
                        this, enemiesPoolObjectsFactory, spawnPoints, enemiesConfigs),
                [typeof(SecondMultiPlayerState)] =
                    new SecondMultiPlayerState(
                        this, enemiesPoolObjectsFactory, spawnPoints, enemiesConfigs)
            };
            
            EnterIn<FirstMultiPlayerState>();
        }
    }
}
