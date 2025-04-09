using GameControllers.Factories.ObjectsFactories;
using GameControllers.Factories.Properties;
using GameControllers.Weapons;
using Zenject;

namespace GameControllers.Installers
{
    public class WeaponFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ICanGetNewObject<Weapon>>()
                .To<WeaponFactory>()
                .AsSingle()
                .NonLazy();
        }
    }
}
