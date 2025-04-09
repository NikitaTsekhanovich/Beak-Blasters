using Containers;
using GameControllers.GameLogic;
using Photon.Pun;
using SaveSystems;
using StartSceneControllers;
using TMPro;
using UnityEngine;

namespace GameControllers.PlayerControllers.PlayerCanvasControllers
{
    public class LoseScreen : MonoBehaviourPun
    {
        [Header("Local player info")]
        [SerializeField] private GameObject _localLoseScreen;
        [SerializeField] private TMP_Text _coinsText;
        [Header("Network player info")]
        [SerializeField] private GameObject _networkLoseScreen;
        [Header("Single player screen")]
        [SerializeField] private GameObject _loseSingleplayerSceen;
        [SerializeField] private TMP_Text _coinsSingleplayerText;
        
        private SaveSystem _saveSystem;
        private SoundsContainer _soundsContainer;

        public void Initialize(
            SaveSystem saveSystem,
            SoundsContainer soundsContainer)
        {
            
            _saveSystem = saveSystem;
            _soundsContainer = soundsContainer;
            
            GameStateController.OnGameOver += ShowLoseScreen;
        }

        private void ShowLoseScreen()
        {
            _soundsContainer.PlayLoseSound();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer && !photonView.IsMine)
                _networkLoseScreen.SetActive(true);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer && photonView.IsMine)
            {
                _localLoseScreen.SetActive(true);
                _coinsText.text = $"{_saveSystem.GameSaveData.PlayerSaveData.Coins}";
            }
            else if (GameModeData.ModeGame == ModeGame.Single)
            {
                _loseSingleplayerSceen.SetActive(true);
                _coinsSingleplayerText.text = $"{_saveSystem.GameSaveData.PlayerSaveData.Coins}";
            }
        }

        private void OnDestroy()
        {
            GameStateController.OnGameOver -= ShowLoseScreen;
        }
    }
}

