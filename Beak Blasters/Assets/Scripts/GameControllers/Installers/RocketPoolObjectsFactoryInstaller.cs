using GameControllers.AttackEntities.Types;
using GameControllers.Factories.PoolObjectsFactories;
using GameControllers.Factories.Properties;
using UnityEngine;
using Zenject;

namespace GameControllers.Installers
{
    public class RocketPoolObjectsFactoryInstaller : MonoInstaller
    {
        [SerializeField] private Rocket _rocketPrefab;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ICanGetPoolEntity<Rocket>>()
                .To<RocketPoolObjectsFactory>()
                .AsSingle()
                .WithArguments(_rocketPrefab)
                .NonLazy();
        }
    }
}
