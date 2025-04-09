using GameControllers.Entities.Enemies.Configs;

namespace Containers
{
    public class EnemiesConfigsContainer
    {
        public EnemyConfig[] EnemiesConfigs { get; private set; }

        public EnemiesConfigsContainer(EnemyConfig[] enemiesConfigs)
        {
            EnemiesConfigs = enemiesConfigs;
        }
    }
}
