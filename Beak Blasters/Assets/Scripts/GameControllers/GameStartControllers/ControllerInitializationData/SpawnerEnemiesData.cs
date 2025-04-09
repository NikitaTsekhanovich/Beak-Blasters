using System;
using System.Collections.Generic;
using GameControllers.Entities;
using GameControllers.Entities.Enemies;
using GameControllers.Entities.Enemies.Configs;
using GameControllers.Levels;
using UnityEngine;

namespace GameControllers.GameStartControllers.ControllerInitializationData
{
    [Serializable]
    public struct SpawnerEnemiesData
    {
        public Transform[] SpawnPoints;
        public Entity<Enemy> EnemyPrefab;
        public LevelConfig[] LocalLevelsConfigs;
        public LevelConfig[] NetworkLevelsConfigs;
    }
}
