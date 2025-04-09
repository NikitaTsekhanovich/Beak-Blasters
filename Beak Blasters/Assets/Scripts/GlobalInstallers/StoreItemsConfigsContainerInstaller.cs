using Containers;
using StartSceneControllers.Store;
using UnityEngine;
using Zenject;

namespace GlobalInstallers
{
    public class StoreItemsConfigsContainerInstaller : MonoInstaller
    {
        [SerializeField] private SkinConfig[] _skinsConfigs;
        [SerializeField] private WeaponConfig[] _weaponsConfigs;
        
        public override void InstallBindings()
        {
            Container
                .Bind<StoreItemsConfigsContainer>()
                .AsSingle()
                .WithArguments(_skinsConfigs, _weaponsConfigs)
                .NonLazy();
        }
    }
}
