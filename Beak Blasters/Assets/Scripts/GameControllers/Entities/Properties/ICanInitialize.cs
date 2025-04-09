using System;
using UnityEngine;

namespace GameControllers.Entities.Properties
{
    public interface ICanInitialize<T>
    {
        public void SpawnInit(Action<Entity<T>> returnAction);
        public void ActiveInit(Vector3 startPosition, Quaternion startRotation);
    }
}
