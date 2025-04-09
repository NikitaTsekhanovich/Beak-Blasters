using DG.Tweening;
using GameControllers.PlayerControllers.Properties;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.Entities.Bonuses
{
    public class BonusPower : Entity<BonusPower>
    {
        private const int StartBonusMaxHealth = 1;
        private const int StartBonusHealHealth = 2;
        private const float DelayAppear = 0.3f;
        
        private int _currentBonusMaxHealth;
        private int _currentBonusHealHealth;

        [PunRPC]
        public void Initialize(int levelEnemy)
        {
            _currentBonusMaxHealth = levelEnemy + StartBonusMaxHealth;
            _currentBonusHealHealth = levelEnemy + StartBonusHealHealth;
            var currentScale = new Vector3(
                levelEnemy + 2,
                levelEnemy + 2,
                levelEnemy + 2);

            Appear(currentScale);
        }

        private void Appear(Vector3 currentScale)
        {
            transform.DOScale(currentScale, DelayAppear);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (GameModeData.ModeGame == ModeGame.Multiplayer && !photonView.IsMine) return;
            
            if (col.TryGetComponent<ICanGetBonus>(out var bonusHandler))
            {
                if (GameModeData.ModeGame == ModeGame.Single || bonusHandler.PhotonView.IsMine)
                    bonusHandler.TakeBonus(_currentBonusMaxHealth, _currentBonusHealHealth);
                else
                    bonusHandler.PhotonView.RPC("TakeBonus", RpcTarget.Others, 
                        _currentBonusMaxHealth, _currentBonusHealHealth);
                        
                ReturnToPool();
            }
        }
        
        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }
        
        [PunRPC]
        private void SyncPosition(Vector3 startPosition, Quaternion startRotation)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }

        [PunRPC]
        private void SendReturnToPool()
        {
            
        }
        
        [PunRPC]
        private void SyncStateEntity(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}

