using GameControllers.Entities.Bonuses;
using GameControllers.Factories.PoolObjectsFactories;
using GameControllers.Factories.Properties;
using UnityEngine;
using Zenject;

namespace GameControllers.Installers
{
    public class BonusPoolObjectsFactoryInstaller : MonoInstaller
    {
        [SerializeField] private BonusPower _bonusPowerPrefab;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ICanGetPoolEntity<BonusPower>>()
                .To<BonusPoolObjectsFactory>()
                .AsSingle()
                .WithArguments(_bonusPowerPrefab)
                .NonLazy();
        }
    }
}
