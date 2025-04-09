using Photon.Pun;
using SaveSystems;

namespace GameControllers.PlayerControllers
{
    public class CoinsController : MonoBehaviourPun
    {
        private int _currentCoins;
        private SaveSystem _saveSystem;
        
        public void Initialize(SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _currentCoins = _saveSystem.GameSaveData.PlayerSaveData.Coins;
        }

        public void IncreaseCoins(int value)
        {
            _currentCoins += value + 1;
            _saveSystem.SavePlayerCoins(_currentCoins);
        }
    }
}

