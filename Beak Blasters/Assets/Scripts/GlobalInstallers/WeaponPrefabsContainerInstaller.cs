using Containers;
using GameControllers.Weapons;
using UnityEngine;
using Zenject;

namespace GlobalInstallers
{
    public class WeaponPrefabsContainerInstaller : MonoInstaller
    {
        [SerializeField] private Weapon[] _weaponsPrefabs;
        
        public override void InstallBindings()
        {
            Container
                .Bind<WeaponPrefabsContainer>()
                .AsSingle()
                .WithArguments(_weaponsPrefabs)
                .NonLazy();
        }
    }
}
