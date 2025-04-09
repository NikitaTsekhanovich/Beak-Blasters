using Containers;
using MusicSystem;
using Photon.Pun;
using SaveSystems;
using SceneSwitchHandlers;
using StartSceneControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.PlayerControllers.PlayerCanvasControllers
{
    public class InterfaceController : MonoBehaviourPun
    {
        [SerializeField] private TMP_Text _coinsPauseBlock;
        [SerializeField] private Button _restartButton;
        [SerializeField] private GameObject _restartText;
        [SerializeField] private Image _currentMusicImage;
        [SerializeField] private Image _currentEffectsImage;
        
        private SceneDataLoader _sceneDataLoader;
        private MusicController _musicController;
        private SaveSystem _saveSystem;
        private SoundsContainer _soundsContainer;

        public void Initialize(
            SceneDataLoader sceneDataLoader, 
            MusicController musicController,
            SaveSystem saveSystem,
            SoundsContainer soundsContainer)
        {
            _sceneDataLoader = sceneDataLoader;
            _musicController = musicController;
            _saveSystem = saveSystem;
            _soundsContainer = soundsContainer;
            
            LoadStartStates();
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }

        private void LoadStartStates()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _restartButton.interactable = true;
                _restartText.SetActive(false);
            }
            else
            {
                _restartButton.interactable = false;
                _restartText.SetActive(true);
            }

            if (GameModeData.ModeGame == ModeGame.Single || photonView.IsMine)
            {
                _musicController.CheckMusicState(_currentMusicImage);
                _musicController.CheckSoundEffectsState(_currentEffectsImage);
            }
        }

        public void BackToMenu()
        {
            if (GameModeData.ModeGame == ModeGame.Single)
            {
                _sceneDataLoader.ChangeScene("StartScene");
            }
            else if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                PhotonNetwork.LeaveRoom();
            }
            
            Time.timeScale = 1f;
        }

        public void PauseGame()
        {
            if (GameModeData.ModeGame == ModeGame.Single)
                Time.timeScale = 0f;
            
            _coinsPauseBlock.text = $"{_saveSystem.GameSaveData.PlayerSaveData.Coins}";
        }

        public void ResumeGame()
        {
            if (GameModeData.ModeGame == ModeGame.Single)
                Time.timeScale = 1f;
        }

        public void RestartGame()
        {
            if (GameModeData.ModeGame == ModeGame.Single)
            {
                StartRestartGame();
            }
            else if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                photonView.RPC("StartRestartGame", RpcTarget.All);
            }
        }

        public void ChangeMusicState()
        {
            _musicController.ChangeMusicState(_currentMusicImage);
        }

        public void ChangeEffectsState()
        {
            _musicController.ChangeSoundEffectsState(_currentEffectsImage);
        }

        public void ClickButton()
        {
            _soundsContainer.PlayClickSound();
        }
        
        [PunRPC]
        private void StartRestartGame()
        {
            _sceneDataLoader.ChangeScene("Game");
        }
    }
}

