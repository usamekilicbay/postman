using UnityEngine;
using UnityEngine.UI;
using Zenject;

public enum PresentInventory
{
    Main,
    Auction,
    Temporary
}

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private Button moveItemButton;
    [SerializeField] private Image image;

    public PresentInventory PresentInventory { get; private set; }

    public InventoryItem InventoryItem { get; private set; }

    private UIManagerBase _uiManager;
    private UIGameScreen _uiGameScreen;
    private UIAuctionPreparationScreen _uiAuctionPreparationScreen;
    private UIGameResultScreen _uiGameResultScreen;

    [Inject]
    public void Construct(UIManagerBase uiManager,
        UIGameScreen uiGameScreen,
        UIAuctionPreparationScreen uiAuctionPreparationScreen,
        UIGameResultScreen uiGameResultScreen)
    {
        _uiManager = uiManager;
        _uiGameScreen = uiGameScreen;
        _uiAuctionPreparationScreen = uiAuctionPreparationScreen;
        _uiGameResultScreen = uiGameResultScreen;
    }

    private void Awake()
    {
        moveItemButton.onClick.AddListener(SendItemToInventoryManager);
    }

    public void SetItem(InventoryItem item)
    {
        InventoryItem = item;
        image.sprite = InventoryItem.ItemConfig.Artwork;
    }

    public void SendItemToInventoryManager()
    {
        switch (PresentInventory)
        {
            case PresentInventory.Main:
                if (_uiManager.GetActiveUIScreen() == _uiAuctionPreparationScreen)
                    _uiAuctionPreparationScreen.MoveItemToAuctionInventory(this);
                else
                    _uiGameResultScreen.MoveItemToTemporaryInventory(this);
                break;
            case PresentInventory.Auction:
                    _uiAuctionPreparationScreen.MoveItemToMainInventory(this);
                break;
            case PresentInventory.Temporary:
                _uiGameResultScreen.MoveItemToMainInventory(this);
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
            case PresentInventory.Temporary:
                _uiGameScreen.DiscardItemFromInventory(this);
                break;
        }
    }

    public void UpdatePresentInventory(PresentInventory presentInventory)
    {
        PresentInventory = presentInventory;
    }

    public class Factory : PlaceholderFactory<UIInventoryItem>
    {

    }
}
