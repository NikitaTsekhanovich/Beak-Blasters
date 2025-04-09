using GameControllers.GameLogic;
using UnityEngine;
using Zenject;

namespace GameControllers.Installers
{
    public class GameStateControllerInstaller : MonoInstaller
    {
        [SerializeField] private AudioSource _loseSound;

        public override void InstallBindings()
        {
            Container
                .Bind<GameStateController>()
                .AsSingle()
                .WithArguments(_loseSound)
                .NonLazy();
        }
    }
}
