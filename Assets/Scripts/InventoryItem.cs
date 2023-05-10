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

    [Inject]
    public void Construct(UIAuctionPreparationScreen uiAuctionPreparationScreen)
    {
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

    public void UpdatePresentInventory(PresentInventory presentInventory)
    {
        PresentInventory = presentInventory;
    }

    public class Factory : PlaceholderFactory<InventoryItem>
    {

    }
}
