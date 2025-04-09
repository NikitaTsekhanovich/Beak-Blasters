using Photon.Pun;

namespace GameControllers.PlayerControllers.Properties
{
    public interface ICanGetBonus
    {
        public PhotonView PhotonView { get; }
        public void TakeBonus(int bonusMaxHealth, int bonusHealHealth);
    }
}
