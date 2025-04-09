using System.Collections.Generic;
using Containers;
using SaveSystems;
using SaveSystems.TypesData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace StartSceneControllers.Store
{
    public class StoreController : MonoBehaviour
    {
        [SerializeField] private Transform _parentItems;
        [SerializeField] private StoreItem _storeItem;
        [SerializeField] private TMP_Text _titleStoreText;
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private GameObject _actionBlock;
        [SerializeField] private TMP_Text _actionButtonText;
        [SerializeField] private Image _playerSkin;
        [SerializeField] private Image _playerWeapon;
        [Header("Info block components")]
        [SerializeField] private TMP_Text _nameItemText;
        [SerializeField] private TMP_Text _characteristicsText;
        
        [Inject] private SaveSystem _saveSystem;
        [Inject] private StoreItemsConfigsContainer _storeItemsConfigsContainer;
        [Inject] private SoundsContainer _soundsContainer;

        private StoreItemsData _currentItemsData;
        private StoreItemConfig[] _currentStoreItems;
        private int _currentCoins;
        private readonly List<StoreItem> _storeItems = new();
        private StoreItemConfig _currentItem;
        private int _currentIndexStoreItem;

        public void ChooseSkinStore()
        {
            _currentItemsData = _saveSystem.GameSaveData.StoreSkinsData;
            _currentStoreItems = _storeItemsConfigsContainer.SkinsConfigs;
            _titleStoreText.text = "Skins";

            SetCurrentData();
        }
        
        public void ChooseWeaponStore()
        {
            _currentItemsData = _saveSystem.GameSaveData.StoreWeaponsData;
            _currentStoreItems = _storeItemsConfigsContainer.WeaponsConfigs;
            _titleStoreText.text = "Weapons";
            
            SetCurrentData();
        }

        private void SetCurrentData()
        {
            _currentCoins = _saveSystem.GameSaveData.PlayerSaveData.Coins;
            UpdateCoinsText();

            for (var i = 0; i < _currentStoreItems.Length; i++)
            {
                var newItem = Instantiate(_storeItem, _parentItems);
                newItem.Init(_currentStoreItems[i].Price, _currentStoreItems[i].GameSprite, 
                    i, ChooseItem, _currentItemsData.StatesItems[i]);

                _storeItems.Add(newItem);
            }
        }

        private void UpdateCoinsText()
        {
            _coinsText.text = $"{_currentCoins}";
        }
        
        private void SelectItem()
        {
            _actionButtonText.text = "Selected";
            
            _saveSystem.SaveStateSkin(_currentItemsData.IndexChosenItem, TypeStateStoreItem.Bought, _currentItemsData);
            _storeItems[_currentItemsData.IndexChosenItem].ChangeChosenState(false);

            _saveSystem.SaveIndexChosenSkin(_currentItem.Index, _currentItemsData);

            _storeItems[_currentItem.Index].ChangeChosenState(true);
            _saveSystem.SaveStateSkin(_currentItem.Index, TypeStateStoreItem.Selected, _currentItemsData);
        }

        private void ChooseItem(int index)
        {
            _soundsContainer.PlayClickSound();
            
            _storeItems[_currentIndexStoreItem].ChangeClickStateItem(false);
            _currentIndexStoreItem = index;
            _storeItems[_currentIndexStoreItem].ChangeClickStateItem(true);
            
            _actionBlock.SetActive(true);
            _currentItem = _currentStoreItems[_currentIndexStoreItem];

            if (_currentItem.TypeState == TypeStateStoreItem.Selected)
            {
                _actionButtonText.text = "Selected";
            }
            else if (_currentItem.TypeState == TypeStateStoreItem.Bought)
            {
                _actionButtonText.text = "Select";
            }
            else if (_currentItem.TypeState == TypeStateStoreItem.NotBought) 
            {
                _actionButtonText.text = "Buy";
            }
        }
        
        public void UpdatePlayerSkins()
        {
            var indexSkin = _saveSystem.GameSaveData.StoreSkinsData.IndexChosenItem;
            var indexWeapon = _saveSystem.GameSaveData.StoreWeaponsData.IndexChosenItem;
            _playerSkin.sprite = _storeItemsConfigsContainer.SkinsConfigs[indexSkin].GameSprite;
            _playerWeapon.sprite = _storeItemsConfigsContainer.WeaponsConfigs[indexWeapon].GameSprite;
        }
        
        public void ClearStore()
        {
            while (_parentItems.childCount > 0) 
                DestroyImmediate(_parentItems.GetChild(0).gameObject);

            _actionBlock.SetActive(false);
            _storeItems.Clear();
            UpdatePlayerSkins();
        }

        public void BuyOrSelectItem()
        {
            if (_currentItem.TypeState == TypeStateStoreItem.Bought || 
                _currentItem.TypeState == TypeStateStoreItem.Selected)
            {
                SelectItem();
            }
            else if (_currentItem.TypeState == TypeStateStoreItem.NotBought) 
            {
                if (_currentCoins - _currentItem.Price >= 0)
                {
                    _soundsContainer.PlayPurchaseSound();
                    _currentCoins -= _currentItem.Price;
                    _saveSystem.SavePlayerCoins(_currentCoins);
                    UpdateCoinsText();
                    _storeItems[_currentItem.Index].HidePriceText();
                    SelectItem();
                }
            }
        }

        public void ClickOpenInfoItem()
        {
            _nameItemText.text = _currentItem.NameText;
            _characteristicsText.text = _currentItem.DescriptionText;
        }
    }
}

