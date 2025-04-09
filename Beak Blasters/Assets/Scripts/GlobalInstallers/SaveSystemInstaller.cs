using SaveSystems;
using Zenject;

namespace GlobalInstallers
{
    public class SaveSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {            
            Container
                .Bind<SaveSystem>()
                .AsSingle()
                .NonLazy();
        }
    }
}
