using UnityEngine;

namespace StartSceneControllers.Store
{
    public class StoreItemConfig : ScriptableObject
    {
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public Sprite GameSprite { get; private set; }
        [field: SerializeField] public string NameText { get; private set; }
        [field: TextArea]
        [field: SerializeField] public string DescriptionText { get; private set; }

        public TypeStateStoreItem TypeState { get; set; }
    }
}

