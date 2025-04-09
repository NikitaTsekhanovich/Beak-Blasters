using GameControllers.AttackEntities.Types;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.Weapons.WeaponsTypes
{
    public class Flamethrower : Weapon
    {
        [SerializeField] private Fire _fire;
        
        protected override void SpawnAttackEntities(Transform throwPoint)
        {
            StartAttack();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("StartAttack", RpcTarget.Others);
        }
        
        public override void Initialize(string ownerTag, int ownerId = -1)
        {
            base.Initialize(ownerTag, ownerId);
            _fire.Initialize(_weaponData.Damage, ownerTag, ownerId);
        }
        
        [PunRPC]
        private void StartAttack()
        {
            _shootSound.Play();
            _fire.StartAttack();
        }
    }
}
