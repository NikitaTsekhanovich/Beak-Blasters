using System;
using GameControllers.GameLogic;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace GameControllers.PlayerControllers
{
    public class TimerRespawn : IDisposable
    {
        private readonly TMP_Text _timerRespawnText;
        private readonly PhotonView _photonView;
        private readonly HealthPlayer _healthPlayer;
        private readonly GameStateController _gameStateController;
        
        private float _currentTime;
        private bool _isTimerActive;
        private bool _isTimerRespawnActive;
        private string _playerName;
        
        private const float TimerStartValue = 10f;
        
        public event Action OnRespawn;

        public TimerRespawn(
            TMP_Text timerRespawnText,
            PhotonView photonView,
            GameStateController gameStateController,
            HealthPlayer healthPlayer)
        {
            _timerRespawnText = timerRespawnText;
            _photonView = photonView;
            _gameStateController = gameStateController;
            _healthPlayer = healthPlayer;

            _gameStateController.OnStopTimerRespawn += StopTimer;
            _gameStateController.OnStartTimerRespawn += StartRespawnTimer;
            _healthPlayer.OnDeathClonePlayer += StartTimer;
        }
        
        public void PlayRespawnTimer()
        {
            if (!_isTimerRespawnActive) return;

            UpdateTime();

            if (_currentTime <= 0)
            {
                StopTimer();
                _photonView.RPC("RespawnPlayer", RpcTarget.All);
                OnRespawn?.Invoke();
            }
        }

        public void PlayTimer()
        {
            if (!_isTimerActive) return;

            UpdateTime();
            
            if (_currentTime <= 0)
            {
                StopTimer();
                OnRespawn?.Invoke();
            }
        }

        private void UpdateTime()
        {
            _currentTime -= Time.deltaTime;
            _timerRespawnText.text = $"{_playerName} will appear in: {(int)_currentTime}";
        }

        private void StartTimer(string playerName)
        {
            SetStartTimeValues(playerName);
            _isTimerActive = true;
        }

        private void StartRespawnTimer(string playerName)
        {
            SetStartTimeValues(playerName);
            _isTimerRespawnActive = true;
        }
        
        private void SetStartTimeValues(string playerName)
        {
            _playerName = playerName;
            _currentTime = TimerStartValue;
            _timerRespawnText.enabled = true;
        }
        
        private void StopTimer()
        {
            _timerRespawnText.enabled = false;
            _isTimerRespawnActive = false;
            _isTimerActive = false;
        }

        public void Dispose()
        {
            if (_gameStateController != null)
            {
                _gameStateController.OnStartTimerRespawn -= StartRespawnTimer;
                _gameStateController.OnStopTimerRespawn -= StopTimer;
            }
            if (_healthPlayer != null)
            {
                _healthPlayer.OnDeathClonePlayer -= StartTimer;
            }
        }
    }
}

