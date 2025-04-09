using System;
using GameControllers.Entities.Properties;
using GameControllers.PlayerControllers.Properties;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.PlayerControllers
{
    public class HealthPlayer : MonoBehaviourPun, ICanTakeDamage, ICanGetBonus
    {
        [SerializeField] private SpriteRenderer _characterSprite;
        [SerializeField] private Sprite _deathSprite;
        [SerializeField] private Image _healthBar;
        [SerializeField] private GameObject _healthFrame;
        [SerializeField] private AudioSource _takeBonusSound;
        [SerializeField] private UICharacteristics _characteristics;
        [SerializeField] private AudioSource _takeDamageSound;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private ParticleSystem _deathParticles;
        
        private SpriteRenderer _weaponSprite;
        private Sprite _aliveSprite;
        private int _currentMaximumHealth;
        private int _currentHealth;
        private int _maximumHealth;
        private int _startHealth;

        public void Initialize(
            SpriteRenderer weaponSprite, 
            Sprite aliveSprite,
            int startHealth,
            int maximumHealth)
        {
            _weaponSprite = weaponSprite;
            _aliveSprite = aliveSprite;
            _currentMaximumHealth = startHealth;
            _maximumHealth = maximumHealth;
            _currentHealth = _currentMaximumHealth;
            _startHealth = startHealth;
        }
        
        public static Action<string> OnSendDeathClonePlayer;
        public event Action<string> OnDeathClonePlayer;
        public event Action<string> OnDeathLocalPlayer;
        public PhotonView PhotonView => PhotonNetwork.IsConnected ? photonView : null;

        [PunRPC]
        public void TakeBonus(int bonusMaxHealth, int bonusHealHealth)
        {
            UseBonus(bonusMaxHealth, bonusHealHealth);
            var bonusBarValue = (_currentMaximumHealth - _startHealth) / (float)(_maximumHealth - _startHealth);
            _characteristics.UpdateLocalPower(bonusBarValue);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                photonView.RPC("UpdateHealthBar", RpcTarget.Others, _currentHealth / (float)_currentMaximumHealth);
                photonView.RPC("UpdateHisPower", RpcTarget.Others, bonusBarValue);
            }
        }
        
        [PunRPC]
        public void TakeDamage(int damage, int ownerId = -1)
        {
            _currentHealth -= damage;
            
            var healthBarValue = _currentHealth / (float)_currentMaximumHealth;
            UpdateHealthBar(healthBarValue);

            if (_currentHealth <= 0)
            {
                OnDeathLocalPlayer?.Invoke(PhotonNetwork.LocalPlayer.NickName);
                DeathPlayer();
            }
            else if (!_takeDamageSound.isPlaying)
            {
                PlayTakeDamageEffects();
            }
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                photonView.RPC("UpdateHealthBar", RpcTarget.Others, healthBarValue);
                
                if (_currentHealth <= 0)
                {
                    photonView.RPC("DeathPlayer", RpcTarget.Others);
                    photonView.RPC("SendDeathEvent", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);
                }
                else if (!_takeDamageSound.isPlaying)
                {
                    photonView.RPC("PlayTakeDamageEffects", RpcTarget.Others);
                }
            }
        }
        
        public void SendDeathClonePlayer(string playerName) =>
            OnDeathClonePlayer?.Invoke(playerName);
        
        private void UseBonus(int bonusMaxHealth, int bonusHealHealth)
        {
            if (_currentMaximumHealth + bonusMaxHealth >= _maximumHealth)
                _currentMaximumHealth = _maximumHealth;
            else
                _currentMaximumHealth += bonusMaxHealth;
            
            if (_currentHealth + bonusHealHealth >= _currentMaximumHealth)
                _currentHealth = _currentMaximumHealth;
            else
                _currentHealth += bonusHealHealth;
            
            if ((GameModeData.ModeGame == ModeGame.Multiplayer && photonView.IsMine) || 
                GameModeData.ModeGame == ModeGame.Single)
            {
                _takeBonusSound.Play();
            }
            
            UpdateHealthBar(_currentHealth / (float)_currentMaximumHealth);
        }

        private void PlayTakeDamageEffects()
        {
            _takeDamageSound.Play();
        }

        [PunRPC]
        private void UpdateHealthBar(float value)
        {
            _healthBar.fillAmount = value;
        }

        [PunRPC]
        private void DeathPlayer()
        {
            _deathSound.Play();
            _deathParticles.Play();
            _characterSprite.sprite = _deathSprite;
            _weaponSprite.enabled = false;
            _healthFrame.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("Death");
        }

        [PunRPC]
        private void SendDeathEvent(string playerName)
        {
            OnSendDeathClonePlayer?.Invoke(playerName);
        }

        [PunRPC]
        private void RespawnPlayer()
        {
            _characterSprite.sprite = _aliveSprite;
            _weaponSprite.enabled = true;
            _healthFrame.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("Default");
            
            _currentHealth = _currentMaximumHealth / 2;

            if (_currentHealth <= 0)
                _currentHealth = 1;
            
            var healthBarValue = _currentHealth / (float)_currentMaximumHealth;
            UpdateHealthBar(healthBarValue);
        }
    }
}

