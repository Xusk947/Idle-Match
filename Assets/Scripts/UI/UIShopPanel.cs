using UnityEngine;
using UnityEngine.UI;

namespace IdleMatch.UI
{
    public class UIShopPanel : MonoBehaviour
    {
        public UIShopPanel Instance { get; private set; }

        private RectTransform _rectTransform, _buttonHolder;

        private Button _upgrade, _premium, _extra;
        private string _currentButton;
        private bool _uiShowed;

        private void Awake()
        {
            Instance = this;
            _rectTransform = GetComponent<RectTransform>();
            GetButtons();
        }

        private void GetButtons()
        {
            _buttonHolder = transform.Find("ButtonHolder").GetComponent<RectTransform>();

            _upgrade = _buttonHolder.Find("Upgrade").GetComponent<Button>();
            _upgrade.onClick.AddListener(OnUpgradeButtonClick);

            _premium= _buttonHolder.Find("Premium").GetComponent<Button>();
            _premium.onClick.AddListener(OnPremiumButtonClick);

            _extra = _buttonHolder.Find("Extra").GetComponent<Button>();
            _extra.onClick.AddListener(OnExtraButtonClick);
        }

        private void OnUpgradeButtonClick()
        {
            ButtonToggleUI(_upgrade);
        }

        private void OnPremiumButtonClick()
        {
            ButtonToggleUI(_premium);
        }

        private void OnExtraButtonClick()
        {
            ButtonToggleUI(_extra);
        }

        private void ButtonToggleUI(Button button)
        {
            if (_currentButton == button.name && _uiShowed) ToggleShopMenu(false);
            else if (!_uiShowed) ToggleShopMenu(true);
            _currentButton = button.name;
        }

        public void ToggleShopMenu(bool show)
        {
            float destination = show ? _rectTransform.rect.height : 0;
            LeanTweenType type = show ? LeanTweenType.easeInQuart : LeanTweenType.easeOutQuart; 
            LeanTween.moveY(_rectTransform, destination, .25f).setEase(type);
            _uiShowed = show;
        }
    }
}