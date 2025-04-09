using UnityEngine;

namespace GameControllers.AttackEntities.Behaviors.FlyBehavior
{
    public interface IFlyBehavior
    {
        public void InitDirectionShoot(Vector3 directionShoot);
        public void Fly();
    }
}
