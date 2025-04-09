using GameControllers.GameStartControllers.ControllerInitializationData;
using GameControllers.PlayerControllers;
using Photon.Pun;
using UnityEngine;
using StartSceneControllers;
using Zenject;
using Object = UnityEngine.Object;

namespace GameControllers.Spawners
{
    public class SpawnerPlayers
    {
        public SpawnerPlayers(SpawnerPlayersData spawnerPlayersData, DiContainer container)
        {
            SpawnPlayer(spawnerPlayersData, container);
        }

        private void SpawnPlayer(SpawnerPlayersData spawnerPlayersData, DiContainer container)
        {
            var playerPrefab = spawnerPlayersData.Player;
            var spawnPositions = spawnerPlayersData.SpawnPositions;

            Player player = null;
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                var position = GetSpawnPosition(GameModeData.IsFirstPlayer ? 0 : 1, spawnPositions);
                player = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity).GetComponent<Player>();
            }
            else if (GameModeData.ModeGame == ModeGame.Single)
            {
                var position = GetSpawnPosition(0, spawnPositions);
                player = Object.Instantiate(playerPrefab, position, Quaternion.identity);
            }
            
            PlayerCamera.CinemachineVirtual.Follow = player?.transform;
            container.Inject(player);
            container.Bind<Player>().FromInstance(player).AsSingle();
        }

        private Vector3 GetSpawnPosition(int indexPoint, Transform[] spawnPositions)
        {
            var spawnPoint = spawnPositions[indexPoint];
            var position = new Vector3(
                spawnPoint.position.x, 
                spawnPoint.position.y, 
                0);

            return position;
        }
    }
}

