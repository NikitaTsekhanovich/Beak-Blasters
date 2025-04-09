using Containers;
using GameControllers.Factories.Properties;
using UnityEngine;
using Zenject;

namespace GameControllers.Factories
{
    public abstract class Factory<T> : ICanGetNewObject<T>
        where T : MonoBehaviour
    {
        [Inject] protected DiContainer _container;
        [Inject] protected WeaponPrefabsContainer _weaponPrefabsContainer;
        
        public abstract T GetNewObject(Transform transform, int indexPrefab = 0);
    }
}
