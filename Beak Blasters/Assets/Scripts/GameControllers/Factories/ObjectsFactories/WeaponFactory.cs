using GameControllers.Weapons;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.Factories.ObjectsFactories
{
    public class WeaponFactory : Factory<Weapon>
    {
        public override Weapon GetNewObject(Transform transform, int indexPrefab)
        {
            Weapon newWeapon = null;

            if (GameModeData.ModeGame == ModeGame.Single)
            {
                newWeapon = Object.Instantiate(
                        _weaponPrefabsContainer.WeaponsPrefabs[indexPrefab], 
                        transform.position, 
                        transform.rotation);
            }
            else
            {
                newWeapon = PhotonNetwork.Instantiate(
                        _weaponPrefabsContainer.WeaponsPrefabs[indexPrefab].name, 
                        transform.position, 
                        transform.rotation)
                    .GetComponent<Weapon>();
            }
            
            newWeapon.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            _container.Inject(newWeapon);
            return newWeapon;
        }
    }
}
