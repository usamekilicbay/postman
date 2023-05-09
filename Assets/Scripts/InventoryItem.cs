using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Button moveItemButton;
    [SerializeField] private Image image;

    private ItemCardConfig _itemConfig;

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
        _itemConfig = item;
        image.sprite = _itemConfig.Artwork;
    }

    public void SendItemToInventoryManager()
    {
        _uiAuctionPreparationScreen.MoveItemToAuctionInventory(this);
    }

    public class Factory : PlaceholderFactory<InventoryItem>
    {

    }
}
