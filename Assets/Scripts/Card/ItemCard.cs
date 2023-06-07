using DG.Tweening;
using Merchant.Common.Types;
using Merchant.Config.Item;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Merchant.Card
{
    public class ItemCard : CardBase
    {
        [Header("Back Details")]
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI weightText;

        private ItemConfig _itemCardConfig;

        protected override void SwipeRight()
        {
            if (IsUsed)
                return;

            IsUsed = true;
            CollectItem();
        }

        private void CollectItem()
        {
            var isNotEnoughMoney = !currencyManager.BuyItem(_itemCardConfig.Price);

            if (isNotEnoughMoney)
            {
                Debug.Log("Can not be buyed");
                return;
            }

            // TODO: Should I check "IsInventoryFull" here?

            Debug.Log($"Collected: {_itemCardConfig.Name}");

            inventoryManager.CollectItem(_itemCardConfig);
            deckManager.SpawnCard();

            transform.DOMoveX(5f, 0.5f)
                .OnComplete(base.SwipeRight);
        }

        public void SetCardConfigs(ItemConfig config)
        {
            _itemCardConfig = config;

            spriteRenderer.sprite = config.Artwork;
            frontSideNameText.SetText(config.Name);
            backSideNameText.SetText(config.Name);
            descriptionText.SetText(config.Description);
            priceText.SetText($"${config.Price}");
            rarityText.SetText(Enum.GetName(typeof(Rarity), config.Rarity));
            weightText.SetText(config.Weight.ToString("F1"));
        }

        public class Factory : PlaceholderFactory<ItemCard>
        {
        }
    }
}
