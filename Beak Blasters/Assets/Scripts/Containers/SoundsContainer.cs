using DG.Tweening;
using SceneSwitchHandlers;
using UnityEngine;

namespace Containers
{
    public class SoundsContainer : MonoBehaviour
    {
        [field: Header("Musics")]
        [field: SerializeField] public AudioSource MenuBackgroundMusic { get; private set; }
        [field: SerializeField] public AudioSource GameBackgroundMusic { get; private set; }
        
        [field: Header("Effects")]
        [field: SerializeField] public AudioSource PurchaseSound { get; private set; }
        [field: SerializeField] public AudioSource ClickSound { get; private set; }
        [field: SerializeField] public AudioSource CompleteWaveSound { get; private set; }
        [field: SerializeField] public AudioSource LoseSound { get; private set; }

        public void PlayMenuBackgroundMusic()
        {
            if (MenuBackgroundMusic.isPlaying) return;
            
            DOTween.Sequence()
                .AppendInterval(LoadingScreenController.TimeLoadScene)
                .AppendCallback(() =>
                {
                    GameBackgroundMusic.Stop();
                    MenuBackgroundMusic.Play();
                });
        }

        public void PlayGameBackgroundMusic()
        {
            if (GameBackgroundMusic.isPlaying) return;
            
            DOTween.Sequence()
                .AppendInterval(LoadingScreenController.TimeLoadScene)
                .AppendCallback(() =>
                {
                    MenuBackgroundMusic.Stop();
                    GameBackgroundMusic.Play();
                });
        }

        public void PlayPurchaseSound() => PurchaseSound.Play();

        public void PlayClickSound() => ClickSound.Play();

        public void PlayCompleteWaveSound() => CompleteWaveSound.Play();

        public void PlayLoseSound() => LoseSound.Play();
    }
}
