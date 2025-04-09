using GameControllers.Weapons;

namespace Containers
{
    public class WeaponPrefabsContainer
    {
        public Weapon[] WeaponsPrefabs { get; private set; }

        public WeaponPrefabsContainer(Weapon[] weaponsPrefabs)
        {
            WeaponsPrefabs = weaponsPrefabs;
        }
    }
}
