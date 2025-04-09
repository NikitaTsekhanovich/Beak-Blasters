using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.PlayerControllers.Data
{
    [Serializable]
    public struct PlayerSettingsData
    {
        public SpriteRenderer Skin;
        public Image[] LocalPlayerIcons;
        public Image[] LocalPlayerWeaponIcons;
        public Image[] NetworkPlayerIcons;
        public Image[] NetworkPlayerWeaponIcons;
        public Image MarkerPlayer;
        public Sprite Marker1;
        public Sprite Marker2;
        public TMP_Text PlayerNameText;
        public GameObject NetworkPlayerBlockInfo;
    }
}
