using Containers;
using DG.Tweening;
using Photon.Pun;
using StartSceneControllers;
using TMPro;
using UnityEngine;

namespace GameControllers.PlayerControllers.PlayerCanvasControllers
{
    public class UIWaveController : MonoBehaviourPun
    {
        [SerializeField] private TMP_Text _waveText;

        private SoundsContainer _soundsContainer;

        public void Initialize(SoundsContainer soundsContainer)
        {
            _soundsContainer = soundsContainer;
        }

        public void ShowWaveText(int numberWave, bool isNextWave)
        {
            if (!isNextWave) return;
            
            SendShowWaveText(numberWave);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("SendShowWaveText", RpcTarget.Others, numberWave);
        }

        [PunRPC]
        private void SendShowWaveText(int numberWave)
        {
            _soundsContainer.PlayCompleteWaveSound();
            
            _waveText.text = $"You have passed wave {numberWave}!";
            
            DOTween.Sequence()
                .Append(_waveText.transform.DOScale(Vector3.one, 0.5f))
                .AppendInterval(1f)
                .Append(_waveText.transform.DOScale(Vector3.zero, 0.5f));
        }

        private void OnDestroy()
        {
            DOTween.Kill(_waveText);
        }
    }
}
