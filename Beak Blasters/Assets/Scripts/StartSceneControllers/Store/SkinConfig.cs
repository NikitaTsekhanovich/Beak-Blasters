using UnityEngine;

namespace StartSceneControllers.Store
{
    [CreateAssetMenu(fileName = "SkinConfig", menuName = "Store item config/ Skin config")]
    public class SkinConfig : StoreItemConfig
    {
        [field: SerializeField] public int StartHealth { get; private set; }
        [field: SerializeField] public int MaximumHealth { get; private set; }
    }
}

