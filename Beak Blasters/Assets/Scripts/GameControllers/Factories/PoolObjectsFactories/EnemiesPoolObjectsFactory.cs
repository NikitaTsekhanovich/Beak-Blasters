using GameControllers.Entities;
using GameControllers.Entities.Enemies;
using UnityEngine;
using Zenject;

namespace GameControllers.Factories.PoolObjectsFactories
{
    public class EnemiesPoolObjectsFactory : PoolObjectsFactory<Enemy>
    {
        private readonly DiContainer _container;
        
        public EnemiesPoolObjectsFactory(Entity<Enemy> entity, DiContainer container) : base(entity)
        {
            _container = container;
        }

        public override Entity<Enemy> GetPoolEntity(Vector3 startPosition, Quaternion startRotation, int indexEnemyConfig)
        {
            var newEnemyEntity = base.GetPoolEntity(startPosition, startRotation);
            var newEnemy = (Enemy)newEnemyEntity;
            _container.Inject(newEnemy);
            newEnemy.GetIndexConfig(indexEnemyConfig);
            
            return newEnemyEntity;
        }
    }
}
