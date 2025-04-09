using Containers;
using GameControllers.PlayerControllers.Data;
using Photon.Pun;
using SaveSystems;
using StartSceneControllers;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.PlayerControllers
{
    public class PlayerSettings : MonoBehaviourPun
    {
        [SerializeField] private PlayerSettingsData _playerSettingsData;

        private SaveSystem _saveSystem;
        private StoreItemsConfigsContainer _storeItemsConfigsContainer;

        public int IndexChosenIndex { get; private set; }

        public void Initialize(
            SaveSystem saveSystem,
            StoreItemsConfigsContainer storeItemsConfigsContainer)
        {
            _saveSystem = saveSystem;
            _storeItemsConfigsContainer = storeItemsConfigsContainer;
        }

        public void SetSkins()
        {
            var skinIndex = _saveSystem.GameSaveData.StoreSkinsData.IndexChosenItem;
            var weaponIndex = _saveSystem.GameSaveData.StoreWeaponsData.IndexChosenItem;
            
            if (GameModeData.ModeGame == ModeGame.Single)
            {
                _playerSettingsData.MarkerPlayer.enabled = false;
                _playerSettingsData.PlayerNameText.enabled = false;
                
                InitLocalSkins(skinIndex, weaponIndex, null, 
                    _playerSettingsData.LocalPlayerIcons, _playerSettingsData.LocalPlayerWeaponIcons);
        
                _playerSettingsData.NetworkPlayerBlockInfo.SetActive(false);

            }
            else if (GameModeData.ModeGame == ModeGame.Multiplayer)
            {
                var playerName = PhotonNetwork.LocalPlayer.NickName;
                
                InitLocalSkins(skinIndex, weaponIndex, playerName, 
                    _playerSettingsData.LocalPlayerIcons, _playerSettingsData.LocalPlayerWeaponIcons);
                
                photonView.RPC("SendPlayerInfo", RpcTarget.Others, skinIndex, weaponIndex, playerName);
                photonView.RPC("SendDestroyComponent", RpcTarget.Others);
            }
            
            Destroy(this);
        }

        private void InitLocalSkins(
            int skinIndex, 
            int weaponIndex, 
            string playerName, 
            Image[] skinIcons, 
            Image[] weaponIcons)
        {
            IndexChosenIndex = skinIndex;
            _playerSettingsData.PlayerNameText.text = playerName;
            
            _playerSettingsData.Skin.sprite = _storeItemsConfigsContainer.SkinsConfigs[skinIndex].GameSprite;
            
            foreach (var skinIcon in skinIcons)
                skinIcon.sprite = _storeItemsConfigsContainer.SkinsConfigs[skinIndex].GameSprite;
            
            foreach (var weaponIcon in weaponIcons)
                weaponIcon.sprite = _storeItemsConfigsContainer.WeaponsConfigs[weaponIndex].GameSprite;
        }
    
        [PunRPC]
        private void SendPlayerInfo(int skinIndex, int weaponIndex, string playerName)
        {
            _playerSettingsData.MarkerPlayer.sprite = _playerSettingsData.Marker2;
            InitLocalSkins(skinIndex, weaponIndex, playerName, 
                _playerSettingsData.NetworkPlayerIcons, _playerSettingsData.NetworkPlayerWeaponIcons);
        }

        [PunRPC]
        private void SendDestroyComponent()
        {
            Destroy(this);
        }
    }
}

