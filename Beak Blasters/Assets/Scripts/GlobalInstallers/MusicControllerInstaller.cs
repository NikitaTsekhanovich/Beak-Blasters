using MusicSystem;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace GlobalInstallers
{
    public class MusicControllerInstaller : MonoInstaller
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Sprite _musicOnImage;
        [SerializeField] private Sprite _musicOffImage;
        [SerializeField] private Sprite _effectsOnImage;
        [SerializeField] private Sprite _effectsOffImage;
        
        public override void InstallBindings()
        {            
            Container
                .Bind<MusicController>()
                .AsSingle()
                .WithArguments(_mixer, _musicOnImage, _musicOffImage, _effectsOnImage, _effectsOffImage)
                .NonLazy();
        }
    }
}
