using Photon.Pun;
using StartSceneControllers;
using StartSceneControllers.Store;
using UnityEngine;

namespace GameControllers.Weapons
{
    public abstract class Weapon : MonoBehaviourPun
    {
        [SerializeField] protected Transform[] _throwPoints;
        [SerializeField] protected WeaponConfig _weaponData;
        [SerializeField] protected AudioSource _shootSound;
        
        private float _currentTime;
        private bool _canShoot;
        
        protected string _ownerTag;
        protected int _ownerId;
    
        private void Awake()
        {
            _currentTime = _weaponData.RateFire;
        }

        private void Shoot()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _weaponData.RateFire)
            {
                _currentTime = 0;

                foreach (var throwPoint in _throwPoints)
                {
                    SpawnAttackEntities(throwPoint);
                }
            }
        }

        protected abstract void SpawnAttackEntities(Transform throwPoint);
        
        public virtual void Initialize(string ownerTag, int ownerId = -1)
        {
            _ownerTag = ownerTag;
            _ownerId = ownerId;
        }

        public void CanShoot(bool canShoot)
        {
            _canShoot = canShoot;
        }

        public void UpdateSystem()
        {
            if (_canShoot && (GameModeData.ModeGame == ModeGame.Single || photonView.IsMine))
                Shoot();
        }
    }
}
