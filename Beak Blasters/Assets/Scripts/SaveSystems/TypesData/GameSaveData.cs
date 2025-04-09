using System;

namespace SaveSystems.TypesData
{
    [Serializable]
    public class GameSaveData
    {
        public MusicSaveData MusicSaveData;
        public PlayerSaveData PlayerSaveData;
        public StoreSkinsData StoreSkinsData;
        public StoreWeaponsData StoreWeaponsData;

        public GameSaveData(
            MusicSaveData musicSaveData, 
            PlayerSaveData playerSaveData,
            StoreSkinsData storeSkinsData,
            StoreWeaponsData storeWeaponsData)
        {
            MusicSaveData = musicSaveData;
            PlayerSaveData = playerSaveData;
            StoreSkinsData = storeSkinsData;
            StoreWeaponsData = storeWeaponsData;
        }
    }
}
