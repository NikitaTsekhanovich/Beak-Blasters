using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.Entities.Enemies
{
    public class HealthEnemy
    {
        private readonly Action _returnToPool;
        private readonly Image _healthBar;
        private readonly AudioSource _takeDamageSound;
        private readonly AudioSource _deathSound;
        private readonly ParticleSystem _deathParticles;
        
        private int _maxHealth;
        private int _currentHealth;

        public HealthEnemy(
            Action returnToPool, 
            Image healthBar, 
            AudioSource takeDamageSound,
            AudioSource deathSound,
            ParticleSystem deathParticles)
        {
            _returnToPool = returnToPool;
            _healthBar = healthBar;
            _takeDamageSound = takeDamageSound;
            _deathSound = deathSound;
            _deathParticles = deathParticles;
        }

        public void SetValues(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
            _healthBar.fillAmount = 1;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            _healthBar.fillAmount = _currentHealth / (float)_maxHealth;

            if (_currentHealth <= 0)
            {
                _returnToPool.Invoke();
                _deathSound.Play();
                _deathParticles.Play();
            }
            else if (!_takeDamageSound.isPlaying)
            {
                _takeDamageSound.Play();
            }
        }
    }
}

