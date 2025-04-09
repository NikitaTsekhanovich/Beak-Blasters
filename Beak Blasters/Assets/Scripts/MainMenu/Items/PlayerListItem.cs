using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace MainMenu.Items
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _playerNameText;

        private Photon.Realtime.Player _player;

        public void SetInfo(Photon.Realtime.Player player)
        {
            _player = player;
            _playerNameText.text = $"Name: {_player.NickName}";
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            if (_player == otherPlayer)
            {
                Destroy(gameObject);
            }
        }
        
        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}

