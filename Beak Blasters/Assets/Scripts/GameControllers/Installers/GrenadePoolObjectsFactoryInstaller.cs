using GameControllers.AttackEntities.Types;
using GameControllers.Factories.PoolObjectsFactories;
using GameControllers.Factories.Properties;
using UnityEngine;
using Zenject;

namespace GameControllers.Installers
{
    public class GrenadePoolObjectsFactoryInstaller : MonoInstaller
    {
        [SerializeField] private Grenade _grenadePrefab;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ICanGetPoolEntity<Grenade>>()
                .To<GrenadePoolObjectsFactory>()
                .AsSingle()
                .WithArguments(_grenadePrefab)
                .NonLazy();
        }
    }
}
