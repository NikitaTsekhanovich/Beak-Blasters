using Containers;
using StartSceneControllers;
using UnityEngine.SceneManagement;

namespace SceneSwitchHandlers
{
    public class SceneDataLoader
    {
        private readonly LoadingScreenController _loadingScreenController;
        private readonly SoundsContainer _soundsContainer;
        private string _nameScene;
        
        public ModeGame ModeGame { get; private set; }
        
        private SceneDataLoader(LoadingScreenController loadingScreenController, SoundsContainer soundsContainer)
        {
            _loadingScreenController = loadingScreenController;
            _soundsContainer = soundsContainer;
            ChangeScene("StartScene");
        }
        
        private void LoadScene()
        {
            SceneManager.LoadSceneAsync(_nameScene);
        }

        public void SetModeGame(ModeGame modeGame)
        {
            ModeGame = modeGame;
        }
        
        public void ChangeScene(string nameScene)
        {
            if (nameScene == "") return;

            switch (nameScene)
            {
                case "StartScene":
                    _soundsContainer.PlayMenuBackgroundMusic();
                    break;
                case "Menu":
                    _soundsContainer.PlayMenuBackgroundMusic();
                    break;
                case "Game":
                    _soundsContainer.PlayGameBackgroundMusic();
                    break;
            }
            
            _nameScene = nameScene;
            _loadingScreenController.StartAnimationFade(LoadScene);
        }
    }
}

