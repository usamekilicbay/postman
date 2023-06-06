using Merchant.Config;
using TMPro;
using UnityEngine;
using Zenject;

namespace Merchant.Card
{
    public class AuctionCard : CardBase
    {
        //[SerializeField] private TextMeshProUGUI offerText;

        //private ItemCardConfig _item;
        //private int _offer;

        //protected override void SwipeRight()
        //{
        //    currencyManager.SellItem(_offer);
        //    inventoryManager.DiscardAuctionItem(_item);
        //    auctionDeckManager.StartNewItemAuction();

        //    base.SwipeRight();
        //}

        //public void SetCardConfigs(ItemCardConfig itemConfig,
        //    string customerName, int offer, Sprite artwork)
        //{
        //    _item = itemConfig;
        //    _offer = offer;

        //    spriteRenderer.sprite = artwork;
        //    frontSideNameText.SetText(customerName);
        //    backSideNameText.SetText(customerName);
        //    offerText.SetText($"${offer}");
        //    //descriptionText.SetText(config.Description);
        //    //rarityText.SetText(Enum.GetName(typeof(Rarity), config.Rarity));
        //    //weightText.SetText(config.Weight.ToString("F1"));
        //}

        public class Factory : PlaceholderFactory<AuctionCard>
        {
        }
    }
}
