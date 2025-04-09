using Photon.Pun;
using SceneSwitchHandlers;
using UnityEngine;
using Zenject;

namespace GameControllers.NetworkLogicControllers
{
    public class RoomController : MonoBehaviourPunCallbacks
    {
        [Inject] private SceneDataLoader _sceneDataLoader;
        
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log("Игрок покинул комнату: " + otherPlayer.NickName);
        }

        public override void OnLeftRoom()
        {
            _sceneDataLoader.ChangeScene("Menu");
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            Debug.Log("Новый MasterClient: " + newMasterClient.NickName);
            PhotonNetwork.LeaveRoom();
        }
    }
}

