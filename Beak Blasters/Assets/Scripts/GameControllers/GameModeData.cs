using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers
{
    public class GameModeData : MonoBehaviour
    {
        public static ModeGame ModeGame { get; private set; }
        public static bool IsFirstPlayer { get; private set; }
        public static int PlayerCount { get; private set; }

        private void Awake()
        {
            InitSettings();
        }

        private void InitSettings()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PlayerCount = 1;
                ModeGame = ModeGame.Single;
                IsFirstPlayer = true;
                return;
            }
            
            PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            ModeGame = PlayerCount == 1 ? ModeGame.Single : ModeGame.Multiplayer;
            
            if (PhotonNetwork.IsMasterClient)
                IsFirstPlayer = true;
        }
    }
}

