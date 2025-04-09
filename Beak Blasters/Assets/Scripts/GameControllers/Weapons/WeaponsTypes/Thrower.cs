using GameControllers.AttackEntities.Types;
using GameControllers.Factories.Properties;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;
using Zenject;

namespace GameControllers.Weapons.WeaponsTypes
{
    public class Thrower : Weapon
    {
        private ICanGetPoolEntity<Grenade> _grenadeFactory;
        
        [Inject]
        private void Construct(ICanGetPoolEntity<Grenade> grenadeFactory)
        {
            _grenadeFactory = grenadeFactory;
        }
        
        protected override void SpawnAttackEntities(Transform throwPoint)
        {
            var newGrenade = (Grenade)_grenadeFactory.GetPoolEntity(throwPoint.position, throwPoint.rotation);
            newGrenade.Initialize(_weaponData.Damage, _ownerTag, _ownerId);
            newGrenade.InitDirectionShoot(_throwPoints[0].position - transform.position);
            
            StartAnimationShoot();
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("StartAnimationShoot", RpcTarget.Others);
        }
        
        [PunRPC]
        private void StartAnimationShoot()
        {
            _shootSound.Play();
        }
    }
}
