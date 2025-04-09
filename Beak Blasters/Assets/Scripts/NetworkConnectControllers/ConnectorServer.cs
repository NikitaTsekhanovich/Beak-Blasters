using Photon.Pun;
using SaveSystems;
using SceneSwitchHandlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NetworkConnectControllers
{
    public class ConnectorServer : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Button _singlePlayerButton;
        [SerializeField] private TMP_InputField _playerNameInput;
        [SerializeField] private TMP_Text _buttonText;
        
        [Inject] private SceneDataLoader _sceneDataLoader;
        [Inject] private SaveSystem _saveSystem;
        
        private const int CharacterNameLimit = 16;

        private void Start()
        {
            _playerNameInput.characterLimit = CharacterNameLimit;
            LoadName();
        }

        private void LoadName()
        {
            var playerSavedName = _saveSystem.GameSaveData.PlayerSaveData.Name;
            _playerNameInput.text = playerSavedName;
            PhotonNetwork.NickName = playerSavedName;
        }
        
        private bool IsInternetAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
        
        public void ConnectToNetwork()
        {
            if (IsInternetAvailable())
            {
                _singlePlayerButton.interactable = false;
                _buttonText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                _buttonText.text = "Check your internet connection";
            }
        }

        public void SaveName()
        {
            var newName = _playerNameInput.text;

            if (newName != "")
            {
                _saveSystem.SavePlayerName(newName);
                PhotonNetwork.NickName = newName;
            }
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _sceneDataLoader.ChangeScene("Menu");
        }
    }
}

