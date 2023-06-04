using Merchant.Common.Abstract;
using Merchant.Inventory;
using Merchant.Manager;
using Merchant.UI.Inventory;
using Merchant.UI.Inventory.Slot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Merchant.UI.Screen
{
    public class UIInventoryScreen : UIScreenBase, IRenewable
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Button homeButton;

        private UIHomeScreen _uiHomeScreen;

        [Inject]
        public void Construct(UIHomeScreen uiHomeScreen)
        {
            _uiHomeScreen = uiHomeScreen;
        }

        private void Awake()
        {
            homeButton.onClick
                .AddListener(ShowHomeScreen);
        }

        public void UpdateMoneyText(int money)
        {
            moneyText.SetText($"${money}");
        }

        public void UpdateRemaningItemCountText(int remaningCardCount)
        {
            moneyText.SetText($"Remaining Cards: {remaningCardCount}");
        }

        private void ShowHomeScreen()
        {
            uiManager.ShowScreen(_uiHomeScreen);
        }

        public override Task Hide()
        {
            Renew();

            return base.Hide();
        }

        public void Renew()
        {
        }
    }
}
