using GameControllers.Entities;
using GameControllers.Entities.Properties;
using GameControllers.Factories.Properties;
using GameControllers.PoolObjects;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.Factories
{
    public abstract class PoolObjectsFactory<T> : ICanGetPoolEntity<T>
        where T : ICanInitialize<T>
    {
        private PoolBase<Entity<T>> _entitiesPool;
        private readonly Entity<T> _entity;
        private const int EntityPreloadCount = 5;

        protected PoolObjectsFactory(Entity<T> entity)
        {
            _entity = entity;
        }
        
        private void ReturnEntity(Entity<T> entity) => _entitiesPool.Return(entity);

        private void GetEntityAction(Entity<T> entity) => entity.ChangeStateEntity(true);
        
        private void ReturnEntityAction(Entity<T> entity) => entity.ChangeStateEntity(false);
        
        protected virtual Entity<T> Preload()
        {
            Entity<T> newEntity = null;
            
            if (GameModeData.ModeGame == ModeGame.Single)
            {
                newEntity = Object.Instantiate(_entity, new Vector3(0, 20, 0), Quaternion.identity);
            }
            else if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                newEntity = PhotonNetwork.Instantiate(_entity.name, new Vector3(0, 20, 0), Quaternion.identity)
                    .GetComponent<Entity<T>>();
            }
            
            newEntity.SpawnInit(ReturnEntity);
            return newEntity;
        }

        public void StartFactory()
        {
            _entitiesPool = new PoolBase<Entity<T>>(Preload, GetEntityAction, ReturnEntityAction, EntityPreloadCount);
        }

        public virtual Entity<T> GetPoolEntity(Vector3 spawnPosition, Quaternion rotation, int indexConfigEntity = 0)
        {
            var newEntity = _entitiesPool.Get();
            newEntity.ActiveInit(spawnPosition, rotation);

            return newEntity;
        }
    }
}
