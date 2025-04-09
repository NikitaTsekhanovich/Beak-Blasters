using System;
using System.Collections;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneSwitchHandlers
{
    public class LoadingScreenController : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [SerializeField] private GraphicRaycaster _loadingScreenBlockClick;
        [SerializeField] private Image _background;
        [SerializeField] private Image _leafsBackground;
        [SerializeField] private TMP_Text _loadingText;
        [SerializeField] private Image _logo;
        
        private const byte StartGameEventCode = 1;
        private const float DelayFadeAnimation = 0.7f;
        
        private Coroutine _loadingTextAnimation;
        
        public const float TimeLoadScene = 2.5f;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            // if (photonEvent.Code == StartGameEventCode)
            // {
            //     StartAnimationFade("");
            // }
        }

        public void StartAnimationFade(Action loadScene)
        {
            _loadingScreenBlockClick.enabled = true;
            
            _loadingTextAnimation = StartCoroutine(StartLoadingTextAnimation());
            _loadingText.DOFade(1f, DelayFadeAnimation);
            _logo.DOFade(1f, DelayFadeAnimation);
            _leafsBackground.DOFade(1f, DelayFadeAnimation);

            DOTween.Sequence()
                .Append(_background.DOFade(1f, DelayFadeAnimation))
                .AppendInterval(1.5f)
                .AppendCallback(loadScene.Invoke)
                .AppendInterval(0.3f)
                .OnComplete(EndAnimationFade);
        }

        private void EndAnimationFade()
        {
            _leafsBackground.DOFade(0f, DelayFadeAnimation);
            _logo.DOFade(0f, DelayFadeAnimation);
            _loadingText.DOFade(0f, DelayFadeAnimation);

            DOTween.Sequence()
                .Append(_background.DOFade(0f, DelayFadeAnimation))
                .AppendCallback(() =>
                {;
                    StopCoroutine(_loadingTextAnimation);
                    _loadingScreenBlockClick.enabled = false;
                });
        }

        private IEnumerator StartLoadingTextAnimation()
        {
            while (true)
            {
                _loadingText.text = "Loading.";
                yield return new WaitForSeconds(0.3f);

                _loadingText.text = "Loading..";
                yield return new WaitForSeconds(0.3f);

                _loadingText.text = "Loading...";
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
