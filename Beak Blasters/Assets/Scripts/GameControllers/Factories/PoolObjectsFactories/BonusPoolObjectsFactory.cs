using GameControllers.Entities;
using GameControllers.Entities.Bonuses;

namespace GameControllers.Factories.PoolObjectsFactories
{
    public class BonusPoolObjectsFactory : PoolObjectsFactory<BonusPower>
    {
        public BonusPoolObjectsFactory(Entity<BonusPower> entity) : base(entity)
        {
        }
    }
}
