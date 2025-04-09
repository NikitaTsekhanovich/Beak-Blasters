using Containers;
using UnityEngine;
using Zenject;

namespace GlobalInstallers
{
    public class SoundsContainerInstaller : MonoInstaller
    {
        [SerializeField] private SoundsContainer _soundsContainerPrefab;
        
        public override void InstallBindings()
        {
            Container
                .Bind<SoundsContainer>()
                .FromComponentInNewPrefab(_soundsContainerPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}
