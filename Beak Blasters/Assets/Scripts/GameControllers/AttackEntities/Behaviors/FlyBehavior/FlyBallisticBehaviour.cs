using UnityEngine;

namespace GameControllers.AttackEntities.Behaviors.FlyBehavior
{
    public class FlyBallisticBehaviour : IFlyBehavior
    {
        private readonly Transform _transform;

        private Vector3 _directionShoot;

        public FlyBallisticBehaviour(Transform transform)
        {
            _transform = transform;
        }
        
        public void InitDirectionShoot(Vector3 directionShoot)
        {
            _directionShoot = directionShoot;
        }

        public void Fly()
        {
            var x = _transform.position.x + _directionShoot.x;
            var y = _transform.position.y + _directionShoot.y;
            
            _transform.position = new Vector3(x, y, 0);
        }
    }
}
