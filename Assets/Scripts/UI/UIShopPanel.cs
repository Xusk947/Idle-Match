using UnityEngine;
using UnityEngine.UI;

namespace IdleMatch.UI
{
    /// <summary>
    /// Represents the UI shop panel in the game.
    /// </summary>
    public class UIShopPanel : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of the UIShopPanel.
        /// </summary>
        public UIShopPanel Instance { get; private set; }

        private RectTransform _rectTransform, _buttonHolder;

        private Button _upgrade, _premium, _extra;
        private string _currentButton;
        private bool _uiShowed = false;

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

            _premium = _buttonHolder.Find("Premium").GetComponent<Button>();
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

        /// <summary>
        /// Toggles the UI shop menu based on the clicked button.
        /// </summary>
        /// <param name="button">The clicked button.</param>
        private void ButtonToggleUI(Button button)
        {
            if (_currentButton == button.name && _uiShowed) ToggleShopMenu(false);
            else if (!_uiShowed) ToggleShopMenu(true);
            _currentButton = button.name;
        }

        /// <summary>
        /// Toggles the display of the shop menu.
        /// </summary>
        /// <param name="show">True to show the shop menu, false to hide it.</param>
        public void ToggleShopMenu(bool show)
        {
            float destination = show ? _rectTransform.rect.height : 0;
            LeanTweenType type = show ? LeanTweenType.easeInQuart : LeanTweenType.easeOutQuart;
            LeanTween.moveY(_rectTransform, destination, .25f).setEase(type);
            _uiShowed = show;
        }
    }
}
