using GameControllers.AttackEntities.Types;
using GameControllers.Factories.PoolObjectsFactories;
using GameControllers.Factories.Properties;
using UnityEngine;
using Zenject;

namespace GameControllers.Installers
{
    public class BulletPoolObjectsFactoryInstaller : MonoInstaller
    {
        [SerializeField] private Bullet _bulletPrefab;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ICanGetPoolEntity<Bullet>>()
                .To<BulletPoolObjectsFactory>()
                .AsSingle()
                .WithArguments(_bulletPrefab)
                .NonLazy();
        }
    }
}
