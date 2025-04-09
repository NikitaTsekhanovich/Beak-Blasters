using GameControllers.AttackEntities.Types;
using GameControllers.Factories.Properties;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;
using Zenject;

namespace GameControllers.Weapons.WeaponsTypes
{
    public class Gun : Weapon
    {
        [SerializeField] private ParticleSystem _shotEffect;
        
        private ICanGetPoolEntity<Bullet> _bulletFactory;
        
        [Inject]
        private void Construct(ICanGetPoolEntity<Bullet> bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

        protected override void SpawnAttackEntities(Transform throwPoint)
        {
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("StartAnimationShoot", RpcTarget.Others);

            StartAnimationShoot();
            var newBullet = (Bullet)_bulletFactory.GetPoolEntity(throwPoint.position, throwPoint.rotation);
            newBullet.Initialize(_weaponData.Damage, _ownerTag, _ownerId);
        }

        [PunRPC]
        private void StartAnimationShoot()
        {
            _shootSound.Play();
            _shotEffect.Play();
        }
    }
}
