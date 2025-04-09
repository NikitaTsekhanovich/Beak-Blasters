using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameControllers.AttackEntities
{
    public class RadiusChecker : MonoBehaviour
    {
        private readonly HashSet<Collider2D> _colliders = new();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            _colliders.Add(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _colliders.Remove(other);
        }
        
        public void Explode(Action<Collider2D> onDealDamage)
        {
            var tempColliders = new List<Collider2D>(_colliders);
            
            foreach (var takeDamageObject in tempColliders)
                onDealDamage.Invoke(takeDamageObject);
        }
        
        public void ClearColliders() => _colliders.Clear();
    }
}
