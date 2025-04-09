using GameControllers.PlayerControllers.PlayerCanvasControllers;
using Photon.Pun;
using SaveSystems;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.GameLogic
{
    public class ScoreController : MonoBehaviourPun
    {
        [SerializeField] private UIScore _uiScore;
        
        private SaveSystem _saveSystem;
        private int _localPlayerScore;
        private int _bestScore;

        public void Initialize(SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _bestScore = _saveSystem.GameSaveData.PlayerSaveData.BestScore;
        }

        public void IncreaseScore(int ownerId)
        {
            if (GameModeData.ModeGame == ModeGame.Multiplayer && photonView.ViewID != ownerId) return;
            
            _localPlayerScore++;
            _uiScore.ChangeLocalPlayerScore(_localPlayerScore);

            if (_localPlayerScore > _bestScore)
            {
                _bestScore = _localPlayerScore;
                _saveSystem.SavePlayerBestScore(_bestScore);
            }
        }
    }
}

