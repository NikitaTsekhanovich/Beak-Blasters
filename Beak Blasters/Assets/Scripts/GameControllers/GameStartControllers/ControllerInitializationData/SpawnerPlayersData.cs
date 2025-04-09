using System;
using GameControllers.PlayerControllers;
using UnityEngine;

namespace GameControllers.GameStartControllers.ControllerInitializationData
{
    [Serializable]
    public struct SpawnerPlayersData
    {
        public Player Player;
        public Transform[] SpawnPositions;
    }
}
