using DG.Tweening;
using UnityEngine;

namespace GameControllers.Entities.Enemies
{
    public class PhysicsMovement
    {
        private const float DelayRotate = 7f;
        
        private float _speed;
        private Sequence _rotatableTween;
        private Quaternion _targetRotation;
        
        private readonly Transform _transform;
        private readonly Transform _targetMovement;

        public PhysicsMovement(Transform targetMovement, Transform transform)
        {
            _targetMovement = targetMovement;
            _transform = transform;
        }

        public void InitSpeed(float speed)
        {
            _speed = speed;
            ChooseDirection();
        }

        public void FixedUpdate()
        {
            Move(_targetMovement.position);
            Rotate(_targetRotation);
        }

        public void SynchronizeMovement(Vector3 targetPosition)
        {
            Move(targetPosition);
        }
        
        public void SynchronizeRotate(Quaternion targetRotation)
        {
            Rotate(targetRotation);
        }

        private void Move(Vector3 targetPosition)
        {
            _transform.position = 
                Vector3.Lerp(targetPosition, _transform.position, Time.fixedDeltaTime * _speed);
        }

        private void Rotate(Quaternion targetRotation)
        {
            _transform.rotation = 
                Quaternion.Lerp(_transform.rotation, targetRotation, Time.deltaTime * _speed);
        }

        private void ChooseDirection()
        {
            _rotatableTween = DOTween.Sequence()
                .AppendInterval(DelayRotate)
                .AppendCallback(() =>
                {
                    var randomAngleZ = Random.Range(0f, 360f);
                    _targetRotation = Quaternion.Euler(0, 0, randomAngleZ);
                })
                .SetLoops(-1, LoopType.Restart);
        }

        public void ChangeDirection(float offset)
        {
            _rotatableTween.Kill();
            _targetRotation = Quaternion.Euler(0, 0, offset - _transform.localEulerAngles.z);
            ChooseDirection();
        }

        public void StopRotate()
        {
            _rotatableTween.Kill();
        }
    }
}

