using GameControllers.AttackEntities.Behaviors.ChangeStateBehaviour;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.AttackEntities.Types
{
    public class Laser : AttackEntity<Laser>
    {
        [SerializeField] private RadiusChecker _radiusChecker;
        
        private const float DamagePerSecond = 0.07f;
        
        private float _currentTime;
        
        protected override void Update()
        {
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
            InitLaser();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("InitLaser", RpcTarget.Others);
        }
        
        public void StartAttack()
        {
            _currentTime = DamagePerSecond;
        }
        
        [PunRPC]
        private void InitLaser()
        {
            _changeStateBehavior = new InfinityLifeBehaviour();
        }
    }
}
