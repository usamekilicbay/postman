using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum PresentInventory
{
    Main,
    Auction,
    Temp
}

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Button moveItemButton;
    [SerializeField] private Image image;

    public PresentInventory PresentInventory { get; private set; }

    public ItemCardConfig ItemConfig { get; private set; }

    private UIAuctionPreparationScreen _uiAuctionPreparationScreen;
    private UIGameScreen _uiGameScreen;

    [Inject]
    public void Construct(UIGameScreen uiGameScreen,
        UIAuctionPreparationScreen uiAuctionPreparationScreen)
    {
        _uiGameScreen = uiGameScreen;
        _uiAuctionPreparationScreen = uiAuctionPreparationScreen;
    }

    private void Awake()
    {
        moveItemButton.onClick.AddListener(SendItemToInventoryManager);
    }

    public void SetItem(ItemCardConfig item)
    {
        ItemConfig = item;
        image.sprite = ItemConfig.Artwork;
    }

    public void SendItemToInventoryManager()
    {
        switch (PresentInventory)
        {
            case PresentInventory.Main:
                _uiAuctionPreparationScreen.MoveItemToAuctionInventory(this);
                break;
            case PresentInventory.Auction:
                _uiAuctionPreparationScreen.MoveItemToMainInventory(this);
                break;
            case PresentInventory.Temp:
                break;
        }
    }

    // TODO: Add Discard button and bind this method to it
    public void DiscardItem()
    {
        switch (PresentInventory)
        {
            case PresentInventory.Main:
                _uiAuctionPreparationScreen.DiscardItemFromInventory(this);
                break;
            case PresentInventory.Auction:
                break;
            case PresentInventory.Temp:
                _uiGameScreen.DiscardItemFromInventory(this);
                break;
        }
    }

    public void UpdatePresentInventory(PresentInventory presentInventory)
    {
        PresentInventory = presentInventory;
    }

    public class Factory : PlaceholderFactory<InventoryItem>
    {

    }
}
