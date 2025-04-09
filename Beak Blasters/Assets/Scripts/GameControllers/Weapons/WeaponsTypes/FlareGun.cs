using GameControllers.AttackEntities.Types;
using GameControllers.Factories.Properties;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;
using Zenject;

namespace GameControllers.Weapons.WeaponsTypes
{
    public class FlareGun : Weapon
    {
        private ICanGetPoolEntity<Rocket> _rocketFactory;
        
        [Inject]
        private void Construct(ICanGetPoolEntity<Rocket> rocketFactory)
        {
            _rocketFactory = rocketFactory;
        }
        
        protected override void SpawnAttackEntities(Transform throwPoint)
        {
            var newRocket = (Rocket)_rocketFactory.GetPoolEntity(throwPoint.position, throwPoint.rotation);
            newRocket.Initialize(_weaponData.Damage, _ownerTag, _ownerId);
            
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
