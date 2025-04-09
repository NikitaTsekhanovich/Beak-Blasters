using GameControllers.AttackEntities.Types;
using GameControllers.Entities;

namespace GameControllers.Factories.PoolObjectsFactories
{
    public class RocketPoolObjectsFactory : PoolObjectsFactory<Rocket>
    {
        public RocketPoolObjectsFactory(Entity<Rocket> entity) : base(entity)
        {
        }
    }
}
