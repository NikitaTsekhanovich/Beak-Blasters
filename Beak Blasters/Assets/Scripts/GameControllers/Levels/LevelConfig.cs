using GameControllers.Entities.Enemies.Configs;
using UnityEngine;

namespace GameControllers.Levels
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Level configs/ Level config")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public int IndexLevel { get; private set; }
        [field: SerializeField] public float LevelDurations { get; private set; }
        [field: SerializeField] public float IntervalSpawn { get; private set; }
        [field: SerializeField] public EnemyConfig[] EnemyConfigs { get; private set; }
    }
}
