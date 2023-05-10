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

    private List<InventorySlot> _mainInventorySlots = new();
    private List<InventorySlot> _auctionInventorySlots = new();
    private List<InventoryItem> _items = new();
    private List<InventoryItem> _auctionItems = new();

    private UIHomeScreen _uiHomeScreen;
    private UIGameScreen _uiGameScreen;
    private InventoryItem.Factory _inventoryItemFactory;

    private InventoryManager _inventoryManager;

    [Inject]
    public void Construct(UIHomeScreen homeScreen,
        UIGameScreen gameScreen,
        InventoryManager inventoryManager,
        InventoryItem.Factory inventoryItemFactory)
    {
        _uiGameScreen = gameScreen;
        _uiHomeScreen = homeScreen;
        _inventoryItemFactory = inventoryItemFactory;
        _inventoryManager = inventoryManager;
    }

    private void Awake()
    {
        homeButton.onClick
            .AddListener(() => uiManager.ShowScreen(_uiHomeScreen));

        startAuctionButton.onClick
            .AddListener(StartAuction);
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
            _items.Add(inventoryItem);
        }
    }

    public void MoveItemToAuctionInventory(InventoryItem inventoryItem)
    {
        var mainInventorySlot = inventoryItem.GetComponentInParent<InventorySlot>();
        var auctionInventorySlot = _auctionInventorySlots.FirstOrDefault(x => !x.IsFull);

        if (auctionInventorySlot == null)
        {
            Debug.Log("Auction inventory is full. New item can not be added!");

            return;
        }

        inventoryItem.transform.SetParent(auctionInventorySlot.transform, false);
        _auctionItems.Add(inventoryItem);
        auctionInventorySlot.FillSlot();
        inventoryItem.UpdatePresentInventory(PresentInventory.Auction);
        _items.Remove(inventoryItem);
        mainInventorySlot.EmptySlot();
    }

    public void MoveItemToMainInventory(InventoryItem inventoryItem)
    {
        var acutionInventorySlot = inventoryItem.GetComponentInParent<InventorySlot>();
        var mainInventorySlot = _mainInventorySlots.First(x => !x.IsFull);
        inventoryItem.transform.SetParent(mainInventorySlot.transform, false);
        _items.Add(inventoryItem);
        mainInventorySlot.FillSlot();
        inventoryItem.UpdatePresentInventory(PresentInventory.Main);
        _auctionItems.Remove(inventoryItem);
        acutionInventorySlot.EmptySlot();
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

    public void StartAuction()
    {
        var items = new List<ItemCardConfig>();
        _items.ForEach(x => items.Add(x.ItemConfig));
        var auctionItems = new List<ItemCardConfig>();
        _auctionItems.ForEach(x => auctionItems.Add(x.ItemConfig));

        _inventoryManager.UpdateInventories(items, auctionItems);

        uiManager.ShowScreen(_uiGameScreen);
    }
}
