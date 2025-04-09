using System;
using System.IO;
using Containers;
using SaveSystems.TypesData;
using UnityEngine;
using StartSceneControllers.Store;
using Zenject;

namespace SaveSystems
{
    public class SaveSystem
    {
        private StoreItemsConfigsContainer _storeItemsConfigsContainer;
        private readonly string _savePath;
        
        private const string SaveFileName = "SaveData.json";

        public GameSaveData GameSaveData { get; private set; }

        [Inject]
        private void Construct(StoreItemsConfigsContainer storeItemsConfigsContainer)
        {
            _storeItemsConfigsContainer = storeItemsConfigsContainer;
            
            LoadData();
            GameSaveData.StoreSkinsData.SetTypesItems(_storeItemsConfigsContainer);
            GameSaveData.StoreWeaponsData.SetTypesItems(_storeItemsConfigsContainer);
        }

        private SaveSystem()
        { 
            #if UNITY_EDITOR
                _savePath = Path.Combine(Application.dataPath, SaveFileName);
            #else 
                _savePath = Path.Combine(Application.persistentDataPath, SaveFileName);
            #endif
        }

        private void SaveData()
        {
            var json = JsonUtility.ToJson(GameSaveData, true);
            
            try
            {
                File.WriteAllText(_savePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e}: void _savePath");
            }
        }

        private void LoadData()
        {
            if (!File.Exists(_savePath))
            {
                CreateSaveFile();
            }
            
            var json = File.ReadAllText(_savePath);
            GameSaveData = JsonUtility.FromJson<GameSaveData>(json);
            GameSaveData.StoreSkinsData.DeserializeData();
            GameSaveData.StoreWeaponsData.DeserializeData();
        }

        private void CreateSaveFile()
        {
            GameSaveData = new GameSaveData(
                new MusicSaveData(),
                new PlayerSaveData(),
                new StoreSkinsData(_storeItemsConfigsContainer),
                new StoreWeaponsData(_storeItemsConfigsContainer));
            
            var json = JsonUtility.ToJson(GameSaveData, true);
            File.WriteAllText(_savePath, json);
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                SaveData();
            }
        }
        
        public void SaveMusicSettings(bool musicEnabled, bool soundEffectsEnabled)
        {
            GameSaveData.MusicSaveData.MusicEnabled = musicEnabled;
            GameSaveData.MusicSaveData.SoundEffectsEnabled = soundEffectsEnabled;
            SaveData();
        }

        public void SavePlayerName(string name) 
        {
            GameSaveData.PlayerSaveData.Name = name;
            SaveData();
        }

        public void SavePlayerCoins(int coins)
        {
            GameSaveData.PlayerSaveData.Coins = coins;
            SaveData();
        }

        public void SavePlayerBestScore(int bestScore)
        {
            GameSaveData.PlayerSaveData.BestScore = bestScore;
            SaveData();
        }

        public void SaveIndexChosenSkin(int index, StoreItemsData storeItemsData)
        {
            storeItemsData.IndexChosenItem = index;
            SaveData();
        }

        public void SaveStateSkin(int index, TypeStateStoreItem newState, StoreItemsData storeItemsData)
        {
            storeItemsData.StatesItems[index] = newState;
            storeItemsData.SerializeData();
            _storeItemsConfigsContainer.SkinsConfigs[index].TypeState = newState;
            SaveData();
        }
    }
}
