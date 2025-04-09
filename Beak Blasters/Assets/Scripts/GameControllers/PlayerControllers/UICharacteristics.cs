using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.PlayerControllers
{
    public class UICharacteristics : MonoBehaviour
    {
        [SerializeField] private Image _localProgressPower;
        [SerializeField] private Image _hisProgressPower;
        [SerializeField] private Image _boostLocalProgress;
        [SerializeField] private Image _boostHisProgress;

        public void UpdateLocalPower(float powerValue)
        {
            _localProgressPower.fillAmount = powerValue;
        }
        
        public void UpdateLocalBoost(float powerValue)
        {
            _boostLocalProgress.fillAmount = powerValue;
        }
        
        public void UpdateHisBoost(float powerValue)
        {
            _boostHisProgress.fillAmount = powerValue;
        }
        
        [PunRPC]
        private void UpdateHisPower(float powerValue)
        {
            _hisProgressPower.fillAmount = powerValue;
        }
    }
}
