using System;

namespace SaveSystems.TypesData
{
    [Serializable]
    public class MusicSaveData 
    {
        public bool MusicEnabled;
        public bool SoundEffectsEnabled;

        public MusicSaveData()
        {
            MusicEnabled = true;
            SoundEffectsEnabled = true;
        }
    }
}
