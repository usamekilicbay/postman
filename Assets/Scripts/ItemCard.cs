using DG.Tweening;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public class ItemCard : Card
{
    [Header("Back Details")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI weightText;

    private ItemCardConfig _itemCardConfig;

    protected override async void SwipeRight()
    {
        await CollectItemAsync();
        
        base.SwipeRight();
    }

    private async Task CollectItemAsync()
    {
        var isNotEnoughMoney = !currencyManager.BuyItem(_itemCardConfig.Price);

        if (isNotEnoughMoney)
        {
            Debug.Log("Can not be buyed");
            return;
        }

        // TODO: Should I check "IsInventoryFull" here?

        Debug.Log("Collected");

        inventoryManager.CollectItem(_itemCardConfig);
        deckManager.SpawnCard();

        await transform.DOMoveX(5f, 0.5f)
             .AsyncWaitForKill();
    }

    public void SetCardConfigs(ItemCardConfig config)
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
