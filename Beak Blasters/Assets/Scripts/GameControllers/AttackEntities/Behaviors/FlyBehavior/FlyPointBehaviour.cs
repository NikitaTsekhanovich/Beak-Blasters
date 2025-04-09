using UnityEngine;

namespace GameControllers.AttackEntities.Behaviors.FlyBehavior
{
    public class FlyPointBehaviour : IFlyBehavior
    {
        private readonly Transform _transform;
        private readonly Transform _movePoint;
        private readonly float _speed;

        public FlyPointBehaviour(Transform transform, Transform movePoint, float speed)
        {
            _transform = transform;
            _movePoint = movePoint;
            _speed = speed;
        }

        public void InitDirectionShoot(Vector3 directionShoot)
        {
            
        }

        public void Fly()
        {
            _transform.position = Vector3.Lerp(_transform.position, _movePoint.position, _speed * Time.deltaTime);
        }
    }
}
