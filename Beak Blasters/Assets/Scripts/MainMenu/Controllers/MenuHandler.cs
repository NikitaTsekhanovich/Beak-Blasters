using Containers;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SceneSwitchHandlers;
using UnityEngine;
using Zenject;

namespace MainMenu.Controllers
{
    public class MenuHandler : MonoBehaviourPunCallbacks
    {
        [Inject] private SceneDataLoader _sceneDataLoader;
        [Inject] private SoundsContainer _soundsContainer;
        
        private const byte StartGameEventCode = 1;

        public void StartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.RaiseEvent(StartGameEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }
            _sceneDataLoader.ChangeScene("Game");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void BackToStartScene()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
        {
            _sceneDataLoader.ChangeScene("StartScene");
        }

        public void ClickButton()
        {
            _soundsContainer.PlayClickSound();
        }
    }
}

