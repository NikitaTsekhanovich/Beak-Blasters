using Containers;
using MusicSystem;
using NetworkConnectControllers;
using SceneSwitchHandlers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace StartSceneControllers
{
    public class StartSceneController : MonoBehaviour
    {
        [SerializeField] private Image _currentMusicImage;
        [SerializeField] private Image _currentEffectsImage;
        [SerializeField] private ConnectorServer _connectorServer;
        [SerializeField] private GameObject _educationBlock;
        
        [Inject] private MusicController _musicController;
        [Inject] private SceneDataLoader _sceneDataLoader;
        [Inject] private SoundsContainer _soundsContainer;

        private void Awake()
        {
            _musicController.CheckMusicState(_currentMusicImage);
            _musicController.CheckSoundEffectsState(_currentEffectsImage);
            
            if (PlayerPrefs.GetInt("Education") == 0)
            {
                _educationBlock.SetActive(true);
                PlayerPrefs.SetInt("Education", 1);
            }
        }

        public void StartSingleGame()
        {  
            _sceneDataLoader.SetModeGame(ModeGame.Single);
            _sceneDataLoader.ChangeScene("Game");
        }

        public void StartMultiplayerGame()
        {
            _sceneDataLoader.SetModeGame(ModeGame.Multiplayer);
            _connectorServer.ConnectToNetwork();
        }

        public void ClickChangeMusicState()
        {
            _musicController.ChangeMusicState(_currentMusicImage);
        }

        public void ClickChangeEffectsState()
        {
            _musicController.ChangeSoundEffectsState(_currentEffectsImage);
        }

        public void ClickButton()
        {
            _soundsContainer.PlayClickSound();
        }
    }
}

