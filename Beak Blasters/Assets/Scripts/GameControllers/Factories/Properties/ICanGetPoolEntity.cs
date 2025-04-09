using GameControllers.Entities;
using GameControllers.Entities.Properties;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.Factories.Properties
{
    public interface ICanGetPoolEntity<T>
        where T : ICanInitialize<T>
    {
        public void StartFactory();
        public Entity<T> GetPoolEntity(Vector3 spawnPosition, Quaternion rotation, int indexConfig = 0);
    }
}
