using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.PlayerControllers
{
    public class BoostHandler : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private PhysicsMovement _movement;
        [SerializeField] private UICharacteristics _characteristics;

        private const float MaxBoostValue = 100f;
        private const float DelayRestore = 0.002f;
        private const float DelaySpend = 0.009f;
        private const float ValueBoost = 15f;
        
        private float _localBoostValue;
        private float _networkBoostValue;
        private bool _boostUsed;
        
        private void SpendBoost()
        {
            if (_localBoostValue > 0)
            {
                _localBoostValue -= Time.fixedTime * DelaySpend;
                _characteristics.UpdateLocalBoost(_localBoostValue / MaxBoostValue);
            }
            else
            {
                StopUseBoost();
            }
        }

        private void RestoreBoost()
        {
            if (_localBoostValue < MaxBoostValue)
            {
                _localBoostValue += Time.fixedTime * DelayRestore;
                _characteristics.UpdateLocalBoost(_localBoostValue / MaxBoostValue);
            }
            else
            {
                _localBoostValue = MaxBoostValue;
            }
        }

        public void UseBoost()
        {
            if (!_boostUsed)
            {
                _boostUsed = true;
                _movement.UseBoost(ValueBoost);
            }
        }

        public void StopUseBoost()
        {
            if (_boostUsed)
            {
                _boostUsed = false;
                _movement.StopBoost();
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_localBoostValue);
            }
            else if (stream.IsReading)
            {
                _networkBoostValue = (float)stream.ReceiveNext();
            }
        }

        public void FixedUpdateSystem()
        {
            if (GameModeData.ModeGame == ModeGame.Multiplayer && !photonView.IsMine)
            {
                _characteristics.UpdateHisBoost(_networkBoostValue / MaxBoostValue);
                return;
            }
            
            if (_boostUsed)
                SpendBoost();
            else
                RestoreBoost();
        }
    }
}

