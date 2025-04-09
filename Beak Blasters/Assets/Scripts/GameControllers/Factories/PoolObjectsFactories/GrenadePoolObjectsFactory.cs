using GameControllers.AttackEntities.Types;
using GameControllers.Entities;

namespace GameControllers.Factories.PoolObjectsFactories
{
    public class GrenadePoolObjectsFactory : PoolObjectsFactory<Grenade>
    {
        public GrenadePoolObjectsFactory(Entity<Grenade> entity) : base(entity)
        {
        }
    }
}
