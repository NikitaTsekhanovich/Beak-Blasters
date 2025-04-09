using Photon.Pun;
using UnityEngine;

namespace GameControllers.PlayerControllers
{
    public class PhysicsMovement : MonoBehaviour, IPunObservable
    {
        private const float StartSpeed = 8f;
         
        private Transform _skinTransform;
        private Rigidbody2D _rigidbody;
        private float _currentSpeed;
        private Vector3 _smooth;
        private Vector3 _movement;
        private float _horizontalJoystick;
        private float _verticalJoystick;
        private Quaternion _angleRotation;

        public void Init(
            Transform skinTransform, 
            Rigidbody2D rigidbody)
        {
            _skinTransform = skinTransform;
            _rigidbody = rigidbody;
            
            _currentSpeed = StartSpeed;
        }

        public void SetJoystickInfo(FixedJoystick joystick)
        {
            _horizontalJoystick = joystick.Horizontal;
            _verticalJoystick = joystick.Vertical;
            _movement = new Vector2(_horizontalJoystick * _currentSpeed, _verticalJoystick * _currentSpeed);
        }

        public void DoRotate()
        {
            if (_horizontalJoystick != 0 || _verticalJoystick != 0)
            {
                var targetAngle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
                var currentAngle = Mathf.LerpAngle(_skinTransform.eulerAngles.z, targetAngle, Time.deltaTime * 10);
                
                _skinTransform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
        }

        public void DoMove()
        {
            _rigidbody.linearVelocity = _movement;
        }

        public void SmoothMovement()
        {
            transform.position = Vector3.Lerp(transform.position, _smooth, Time.deltaTime * 10);
            _skinTransform.rotation = Quaternion.Lerp(_skinTransform.rotation, _angleRotation, Time.deltaTime * 10);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(_skinTransform.rotation);
            }
            else if (stream.IsReading)
            {
                _smooth = (Vector3)stream.ReceiveNext();
                _angleRotation = (Quaternion)stream.ReceiveNext();
            }
        }

        public void UseBoost(float valueBoost)
        {
            _currentSpeed = valueBoost;
        }

        public void StopBoost()
        {
            _currentSpeed = StartSpeed;
        }
    }
}

