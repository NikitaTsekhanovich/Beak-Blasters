using GameControllers.AttackEntities.Behaviors.ChangeStateBehaviour;
using GameControllers.AttackEntities.Behaviors.FlyBehavior;
using GameControllers.Entities;
using GameControllers.Entities.Properties;
using GameControllers.PlayerControllers;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.AttackEntities
{
    public abstract class AttackEntity<T> : Entity<T>, ICanDealDamage
        where T : AttackEntity<T>
    {
        [SerializeField] protected float _lifeTime;
        
        private string _ownerTag;
        private int _ownerId;
        private int _damage;
        
        protected IFlyBehavior _flyBehavior;
        protected ICanChangeStateBehavior _changeStateBehavior;

        protected virtual void Update()
        {
            _changeStateBehavior?.CheckActiveTime();
        }

        private void FixedUpdate()
        {
            _flyBehavior?.Fly();
        }
        
        public virtual void DealDamage(Collider2D other)
        {
            if (string.IsNullOrEmpty(_ownerTag)) return;
            
            if (!other.CompareTag(_ownerTag) && other.TryGetComponent(out ICanTakeDamage takeDamageComponent))
            {
                if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
                {
                    if (GameModeData.ModeGame == ModeGame.Single || player.photonView.IsMine)
                        takeDamageComponent.TakeDamage(_damage);
                    else
                        player.photonView.RPC("TakeDamage", RpcTarget.Others, _damage, -1);
                }
                else if (other.CompareTag("Enemy"))
                {
                    takeDamageComponent.TakeDamage(_damage, _ownerId);
                }
                
                DoDestroy();
            }
        }

        protected virtual void DoDestroy()
        {
            ReturnToPool();
        }

        public virtual void Initialize(int damage, string ownerTag, int ownerId = -1)
        {
            _damage = damage;
            _ownerTag = ownerTag;
            _ownerId = ownerId;
        }
    }
}
