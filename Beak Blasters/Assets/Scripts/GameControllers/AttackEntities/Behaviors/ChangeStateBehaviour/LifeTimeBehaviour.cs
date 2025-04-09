using System;
using UnityEngine;

namespace GameControllers.AttackEntities.Behaviors.ChangeStateBehaviour
{
    public class LifeTimeBehaviour : ICanChangeStateBehavior
    {
        private readonly Action _onEndLife;
        private readonly float _lifeTime;
        
        private float _currentTime;
        private bool _isActive;
        
        public LifeTimeBehaviour(Action onEndLife, float lifeTime)
        {
            _onEndLife = onEndLife;
            _lifeTime = lifeTime;
            _isActive = true;
        }
        
        public void CheckActiveTime()
        {
            if (!_isActive) return;
            
            _currentTime += Time.deltaTime;
  
            if (_currentTime >= _lifeTime)
            {
                _onEndLife.Invoke();
                _currentTime = 0;
            }
        }

        public void ChangeStateTimer(bool isActive)
        {
            _currentTime = 0;
            _isActive = isActive;
        }
    }
}
