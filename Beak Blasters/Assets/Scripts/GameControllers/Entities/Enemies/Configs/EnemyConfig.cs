using UnityEngine;

namespace GameControllers.Entities.Enemies.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemies configs/ Enemy config")]
    public class EnemyConfig : ScriptableObject
    {
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public int LevelEnemy { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int[] AvailableIndexesWeapons { get; private set; }
    }
}
