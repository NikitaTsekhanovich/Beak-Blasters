using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StartSceneControllers.Store
{
    public class StoreItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _chooseIcon;
        [SerializeField] private Image _coinImage;
        [SerializeField] private Image _frameChosen;
        private int _index;
        private Action<int> _onChooseItem;

        public void Init(int price, Sprite icon, int index, Action<int> onChooseItem, TypeStateStoreItem typeState)
        {
            _priceText.text = price.ToString();
            _icon.sprite = icon;
            _index = index;
            _onChooseItem = onChooseItem;

            if (typeState == TypeStateStoreItem.Bought || typeState == TypeStateStoreItem.Selected)
                HidePriceText();
            if (typeState == TypeStateStoreItem.Selected)
                ChangeChosenState(true);
        }

        public void ChooseItem()
        {
            _onChooseItem.Invoke(_index);
            ChangeClickStateItem(true);
        }

        public void ChangeClickStateItem(bool isClicked)
        {
            _frameChosen.enabled = isClicked;
        }

        public void ChangeChosenState(bool isChosen)
        {
            _chooseIcon.enabled = isChosen;
        }

        public void HidePriceText()
        {
            _priceText.enabled = false;
            _coinImage.enabled = false;
        }
    }
}

