using TMPro;
using UnityEngine;
using Zenject;

public class AuctionCard : Card
{
    [SerializeField] private TextMeshProUGUI offerText;

    private InventoryItem _item;
    private int _offer;

    protected override void SwipeRight()
    {
        currencyManager.SellItem(_offer);
        inventoryManager.DiscardItem(_item);
        auctionDeckManager.StartNewItemAuction();

        base.SwipeRight();
    }

    public void SetCardConfigs(InventoryItem item, 
        string customerName, int offer, Sprite artwork)
    {
        _item = item;
        _offer = offer;

        spriteRenderer.sprite = artwork;
        frontSideNameText.SetText(customerName);
        backSideNameText.SetText(customerName);
        offerText.SetText($"${offer}");
        //descriptionText.SetText(config.Description);
        //rarityText.SetText(Enum.GetName(typeof(Rarity), config.Rarity));
        //weightText.SetText(config.Weight.ToString("F1"));
    }

    public class Factory : PlaceholderFactory<AuctionCard>
    {
    }
}
