using Containers;
using SceneSwitchHandlers;
using UnityEngine;
using Zenject;

namespace GlobalInstallers
{
    public class SceneDataLoaderInstaller : MonoInstaller
    {
        [SerializeField] private LoadingScreenController _loadingScreenController;
        
        public override void InstallBindings()
        {
            var loadingScreenController = Instantiate(_loadingScreenController);
            var soundsContainer = Container.Resolve<SoundsContainer>();
            
            Container
                .Bind<SceneDataLoader>()
                .AsSingle()
                .WithArguments(loadingScreenController, soundsContainer)
                .NonLazy();
        }
    }
}
