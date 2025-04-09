using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.PlayerControllers.PlayerCanvasControllers
{
    public class CanvasController : MonoBehaviourPun
    {
        [SerializeField] private GameObject _localPlayerCanvas;
        [SerializeField] private GameObject _networkPlayerCanvas;

        private void Start()
        {
            if (photonView.IsMine || GameModeData.ModeGame == ModeGame.Single)
            {
                _localPlayerCanvas.SetActive(true);
                _networkPlayerCanvas.SetActive(false);
            }
            else
            {
                _localPlayerCanvas.SetActive(false);
                _networkPlayerCanvas.SetActive(true);
            }
        }
    }
}

