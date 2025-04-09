using Cinemachine;
using UnityEngine;

namespace GameControllers.PlayerControllers
{
    public class PlayerCamera : MonoBehaviour
    {
        public static CinemachineVirtualCamera CinemachineVirtual { get; private set; }

        private void Awake()
        {
            CinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
        }
    }
}

