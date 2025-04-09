using GameControllers.AttackEntities.Types;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.Weapons.WeaponsTypes
{
    public class LaserGun : Weapon
    {
        [SerializeField] private Laser _laser;

        private void Start()
        {
            _shootSound.Play();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("StartSoundAttack", RpcTarget.Others);
        }

        protected override void SpawnAttackEntities(Transform throwPoint)
        {
            StartAttack();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("StartAttack", RpcTarget.Others);
        }
        
        public override void Initialize(string ownerTag, int ownerId = -1)
        {
            base.Initialize(ownerTag, ownerId);
            _laser.Initialize(_weaponData.Damage, ownerTag, ownerId);
        }
        
        [PunRPC]
        private void StartAttack()
        {
            _laser.StartAttack();
        }

        [PunRPC]
        private void StartSoundAttack()
        {
            _shootSound.Play();
        }
    }
}
