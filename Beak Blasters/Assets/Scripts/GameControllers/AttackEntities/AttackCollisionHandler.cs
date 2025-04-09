using UnityEngine;

namespace GameControllers.AttackEntities
{
    public class AttackCollisionHandler : MonoBehaviour
    {
        private ICanDealDamage _dealerDamage;

        private void Awake()
        {
            _dealerDamage = GetComponentInParent<ICanDealDamage>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _dealerDamage.DealDamage(other);
        }
    }
}
