using UnityEngine;

namespace GameControllers.AttackEntities
{
    public interface ICanDealDamage
    {
        public void DealDamage(Collider2D other);
    }
}
