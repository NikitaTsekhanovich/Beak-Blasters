using System;

namespace SaveSystems.TypesData
{
    [Serializable]
    public class PlayerSaveData
    {
        public string Name;
        public int Coins;
        public int BestScore;

        public PlayerSaveData()
        {
            Name = "Anonymous";
            Coins = 0;
            BestScore = 0;
        }
    }
}
