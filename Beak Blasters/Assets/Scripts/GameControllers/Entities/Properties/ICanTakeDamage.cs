namespace GameControllers.Entities.Properties
{
    public interface ICanTakeDamage
    {
        public void TakeDamage(int damage, int ownerId = -1);
    }
}

