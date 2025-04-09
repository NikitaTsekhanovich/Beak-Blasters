using Photon.Pun;
using StartSceneControllers;
using TMPro;
using UnityEngine;

namespace GameControllers.PlayerControllers.PlayerCanvasControllers
{
    public class UIScore : MonoBehaviourPun
    {
        [SerializeField] private TMP_Text _networkPlayerScoreText;
        [SerializeField] private TMP_Text _localPlayerScoreText;
        [SerializeField] private TMP_Text _networkPlayerScoreLoseScreenText;
        [SerializeField] private TMP_Text _localPlayerScoreLoseScreenText;
        [SerializeField] private TMP_Text _singlePlayerScoreText;

        public void ChangeLocalPlayerScore(int localPlayerScore)
        {
            _localPlayerScoreText.text = $"{localPlayerScore}";
            _localPlayerScoreLoseScreenText.text = $"{localPlayerScore}";
            _singlePlayerScoreText.text = $"{localPlayerScore}";
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("SendScore", RpcTarget.Others, localPlayerScore);
        }

        [PunRPC]
        private void SendScore(int totalScore)
        {
            _networkPlayerScoreText.text = $"{totalScore}";
            _networkPlayerScoreLoseScreenText.text = $"{totalScore}";
        }
    }
}

