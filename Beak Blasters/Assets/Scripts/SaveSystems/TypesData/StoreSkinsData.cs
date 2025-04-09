using System;
using Containers;
using StartSceneControllers.Store;

namespace SaveSystems.TypesData
{
    [Serializable]
    public class StoreSkinsData : StoreItemsData
    {
        public StoreSkinsData(StoreItemsConfigsContainer storeItemsConfigsContainer) : base(storeItemsConfigsContainer)
        {
        }

        public override void SetTypesItems(StoreItemsConfigsContainer storeItemsConfigsContainer)
        {
            for (var i = 0; i < storeItemsConfigsContainer.SkinsConfigs.Length; i++)
                storeItemsConfigsContainer.SkinsConfigs[i].TypeState = StatesItems[i];
        }
    }
}
