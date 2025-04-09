using GameControllers.AttackEntities.Types;
using GameControllers.Entities;

namespace GameControllers.Factories.PoolObjectsFactories
{
    public class BulletPoolObjectsFactory : PoolObjectsFactory<Bullet>
    {
        public BulletPoolObjectsFactory(Entity<Bullet> entity) : base(entity)
        {
        }
    }
}
