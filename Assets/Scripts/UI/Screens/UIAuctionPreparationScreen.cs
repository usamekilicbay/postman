using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIAuctionPreparationScreen : UIScreenBase
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button startAuctionButton;
    [Space(10)]
    [Header("Prefabs")]
    [SerializeField] private GameObject inventorySlotPrefab;
    [Space(10)]
    [Header("Inventories")]
    [SerializeField] private Transform mainInventorySpawnParent;
    [SerializeField] private Transform auctionInventorySpawnParent;

    private List<InventorySlot> _mainInventorySlots;
    private List<InventorySlot> _auctionInventorySlots;

    private UIHomeScreen _uiHomeScreen;
    private UIGameScreen _uiGameScreen;
    private InventoryItem.Factory _inventoryItemFactory;

    [Inject]
    public void Construct(UIHomeScreen homeScreen,
        UIGameScreen gameScreen,
        InventoryItem.Factory inventoryItemFactory)
    {
        _uiGameScreen = gameScreen;
        _uiHomeScreen = homeScreen;
        _inventoryItemFactory = inventoryItemFactory;
    }

    private void Awake()
    {
        _mainInventorySlots = new List<InventorySlot>();
        _auctionInventorySlots = new List<InventorySlot>();

        homeButton.onClick
            .AddListener(() => uiManager.ShowScreen(_uiHomeScreen));

        startAuctionButton.onClick
            .AddListener(() => uiManager.ShowScreen(_uiGameScreen));
    }

    public void LoadItemsToInventory(IReadOnlyList<ItemCardConfig> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            ItemCardConfig item = items[i];
            var inventorySlot = _mainInventorySlots[i];
            inventorySlot.FillSlot();
            var inventoryItem = _inventoryItemFactory.Create().GetComponent<InventoryItem>();
            inventoryItem.transform.SetParent(inventorySlot.transform, false);
            inventoryItem.SetItem(item);
        }
    }

    public void MoveItemToAuctionInventory(InventoryItem inventoryItem)
    {
        var inventorySlot = _auctionInventorySlots.First(x => x.IsEmpty);
        inventoryItem.transform.SetParent(inventorySlot.transform, false);
    }

    public void CreateMainInventorySlots(int inventorySlotCount)
    {
        for (var i = 0; i < inventorySlotCount; i++)
        {
            var inventorySlot = Instantiate(inventorySlotPrefab, mainInventorySpawnParent)
                .GetComponent<InventorySlot>();
            _mainInventorySlots.Add(inventorySlot);
        }
    }

    public void CreateAuctionInventorySlots(int inventorySlotCount)
    {
        for (var i = 0; i < inventorySlotCount; i++)
        {
            var inventorySlot = Instantiate(inventorySlotPrefab, auctionInventorySpawnParent)
                .GetComponent<InventorySlot>();
            _auctionInventorySlots.Add(inventorySlot);
        }
    }
}
