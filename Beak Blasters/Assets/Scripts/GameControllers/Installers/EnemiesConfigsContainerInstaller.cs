using Containers;
using GameControllers.Entities.Enemies.Configs;
using UnityEngine;
using Zenject;

namespace GameControllers.Installers
{
    public class EnemiesConfigsContainerInstaller : MonoInstaller
    {
        [SerializeField] private EnemyConfig[] _enemyConfigs;
        
        public override void InstallBindings()
        {
            Container
                .Bind<EnemiesConfigsContainer>()
                .AsSingle()
                .WithArguments(_enemyConfigs)
                .NonLazy();
        }
    }
}
