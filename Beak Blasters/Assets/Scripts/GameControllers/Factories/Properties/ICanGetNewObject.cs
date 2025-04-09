using UnityEngine;

namespace GameControllers.Factories.Properties
{
    public interface ICanGetNewObject<T>
    {
        public T GetNewObject(Transform transform, int indexPrefab = 0);
    }
}
