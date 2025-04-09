using System;
using GameControllers.PlayerControllers;
using UnityEngine;

namespace GameControllers.GameLogic
{
    public class GameStateController
    {
        private readonly AudioSource _loseSound;
        
        private int _playerCount;
        
        public static Action OnGameOver;

        public GameStateController(AudioSource loseSound)
        {
            _loseSound = loseSound;
            _playerCount = GameModeData.PlayerCount;
        }
        
        public event Action<string> OnStartTimerRespawn;
        public event Action OnStopTimerRespawn;

        private void IncreasePlayers()
        {
            _playerCount++;
        }

        private void DecreasePlayers(string playerName)
        {
            _playerCount--;
        }

        private void CheckStateGame(string playerName)
        {
            DecreasePlayers(playerName);
            
            if (_playerCount <= 0)
            {
                _loseSound.Play();
                OnStopTimerRespawn?.Invoke();
                OnGameOver?.Invoke();
            }
            else 
            {
                OnStartTimerRespawn?.Invoke(playerName);
            }
        }
        
        public void UpdatePlayerCount(int playerCount)
        {
            _playerCount = playerCount;
        }

        public void SubscribeToEvents(HealthPlayer healthPlayer, TimerRespawn timerRespawn)
        {
            healthPlayer.OnDeathLocalPlayer += CheckStateGame;
            healthPlayer.OnDeathClonePlayer += CheckStateGame;
            timerRespawn.OnRespawn += IncreasePlayers;
        }
    }
}

