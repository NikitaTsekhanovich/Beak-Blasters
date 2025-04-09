using UnityEngine;

namespace StartSceneControllers.Store
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Store item config/ Weapon config")]
    public class WeaponConfig : StoreItemConfig
    {
        [field: Header("Weapon settings")]
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float RateFire { get; private set; }
    }
}

