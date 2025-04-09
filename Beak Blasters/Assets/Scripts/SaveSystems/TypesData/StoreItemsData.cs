using System;
using System.Collections.Generic;
using Containers;
using StartSceneControllers.Store;
using Newtonsoft.Json;

namespace SaveSystems.TypesData
{
    [Serializable]
    public abstract class StoreItemsData
    {
        public int IndexChosenItem;
        public Dictionary<int, TypeStateStoreItem> StatesItems;
        public string SerializableStatesItems;

        public StoreItemsData(StoreItemsConfigsContainer storeItemsConfigsContainer)
        {
            IndexChosenItem = 0;
            
            StatesItems = new()
            {
                { IndexChosenItem, TypeStateStoreItem.Selected }
            };
            
            for (var i = 1; i < storeItemsConfigsContainer.SkinsConfigs.Length; i++)
                StatesItems[i] = TypeStateStoreItem.NotBought;

            SerializeData();
        }

        public abstract void SetTypesItems(StoreItemsConfigsContainer storeItemsConfigsContainer);

        public void SerializeData()
        {
            SerializableStatesItems = JsonConvert.SerializeObject(StatesItems);
        }

        public void DeserializeData()
        {
            StatesItems = JsonConvert.DeserializeObject<Dictionary<int, TypeStateStoreItem>>
                (SerializableStatesItems);
        }

        public TypeStateStoreItem GetStateItem(int index)
        {
            return StatesItems[index];
        }
    }
}
