namespace GameControllers.AttackEntities.Behaviors.ChangeStateBehaviour
{
    public interface ICanChangeStateBehavior
    {
        public void CheckActiveTime();
        public void ChangeStateTimer(bool isActive);
    }
}
