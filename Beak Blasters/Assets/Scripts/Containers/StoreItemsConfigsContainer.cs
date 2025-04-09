using StartSceneControllers.Store;

namespace Containers
{
    public class StoreItemsConfigsContainer
    {
        public SkinConfig[] SkinsConfigs { get; private set; }
        public WeaponConfig[] WeaponsConfigs { get; private set; }

        public StoreItemsConfigsContainer(SkinConfig[] skinsConfigs, WeaponConfig[] weaponsConfigs)
        {
            SkinsConfigs = skinsConfigs;
            WeaponsConfigs = weaponsConfigs;
        }
    }
}
