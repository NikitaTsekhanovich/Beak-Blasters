using GameControllers.AttackEntities.Behaviors.ChangeStateBehaviour;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.AttackEntities.Types
{
    public class Fire : AttackEntity<Fire>
    {
        [SerializeField] private RadiusChecker _radiusChecker;
        [SerializeField] private ParticleSystem _attackParticle;

        private const float DamagePerSecond = 0.12f;

        private float _currentTime;
        private bool _isAttacking;

        private void EndAttack()
        {
            SyncEndAttack();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("SyncEndAttack", RpcTarget.Others);
        }

        protected override void Update()
        {
            if (!_isAttacking) return;
            
            base.Update();
            
            _currentTime += Time.deltaTime;

            if (_currentTime >= DamagePerSecond)
            {
                _currentTime = 0f;
                _radiusChecker.Explode(DealDamage);
            }
        }

        protected override void DoDestroy()
        {
        }

        public override void Initialize(int damage, string ownerTag, int ownerId = -1)
        {
            base.Initialize(damage, ownerTag, ownerId);
            InitFire();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("InitFire", RpcTarget.Others);
        }
        
        public void StartAttack()
        {
            _changeStateBehavior.ChangeStateTimer(true);
            _isAttacking = true;
            _currentTime = DamagePerSecond;
            _attackParticle.Play();
        }
        
        [PunRPC]
        private void InitFire()
        {
            _changeStateBehavior = new LifeTimeBehaviour(EndAttack, _lifeTime);
        }

        [PunRPC]
        private void SyncEndAttack()
        {
            _changeStateBehavior.ChangeStateTimer(false);
            _isAttacking = false;
            _currentTime = 0;
            _attackParticle.Stop();
            _radiusChecker.ClearColliders();
        }
    }
}
