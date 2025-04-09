using System;
using Containers;

namespace SaveSystems.TypesData
{
    [Serializable]
    public class StoreWeaponsData : StoreItemsData
    {
        public StoreWeaponsData(StoreItemsConfigsContainer storeItemsConfigsContainer) : base(storeItemsConfigsContainer)
        {
        }

        public override void SetTypesItems(StoreItemsConfigsContainer storeItemsConfigsContainer)
        {
            for (var i = 0; i < storeItemsConfigsContainer.SkinsConfigs.Length; i++)
                storeItemsConfigsContainer.WeaponsConfigs[i].TypeState = StatesItems[i];
        }
    }
}
