using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIGameResultScreen : UIScreenBase, IRenewable
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button auctionButton;
    [Space(10)]
    [Header("Prefabs")]
    [SerializeField] private GameObject inventorySlotPrefab;
    [Space(10)]
    [Header("Inventories")]
    [SerializeField] private Transform mainInventorySpawnParent;
    [SerializeField] private Transform temporaryInventorySpawnParent;

    private List<InventorySlot> _inventorySlots = new();
    private List<InventorySlot> _temporaryInventorySlots = new();
    private List<InventoryItem> _inventoryItems = new();
    private List<InventoryItem> _temporaryInventoryItems = new();

    private GameManager _gameManager;
    private InventoryManager _inventoryManager;
    private UIHomeScreen _uiHomeScreen;
    private UIGameScreen _uiGameScreen;
    private InventoryItem.Factory _inventoryItemFactory;

    [Inject]
    public void Construct(GameManager gameManager,
        InventoryManager inventoryManager,
        UIHomeScreen homeScreen,
        UIGameScreen gameScreen,
        InventoryItem.Factory inventoryItemFactory)
    {
        _gameManager = gameManager;
        _inventoryManager = inventoryManager;
        _uiGameScreen = gameScreen;
        _uiHomeScreen = homeScreen;
        _inventoryItemFactory = inventoryItemFactory;
    }

    private void Awake()
    {
        homeButton.onClick
            .AddListener(GoToHomeScreen);

        auctionButton.onClick
            .AddListener(GoToAuctionPreparationScreen);
    }

    public void LoadItemsToInventory(IReadOnlyList<ItemCardConfig> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            ItemCardConfig item = items[i];
            var inventorySlot = _inventorySlots[i];
            inventorySlot.FillSlot();
            var inventoryItem = _inventoryItemFactory.Create().GetComponent<InventoryItem>();
            inventoryItem.transform.SetParent(inventorySlot.transform, false);
            inventoryItem.SetItem(item);
            inventoryItem.UpdatePresentInventory(PresentInventory.Main);
            _inventoryItems.Add(inventoryItem);
        }
    }

    public void LoadItemsToTemporaryInventory(IReadOnlyList<ItemCardConfig> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            ItemCardConfig item = items[i];
            var inventorySlot = _temporaryInventorySlots[i];
            inventorySlot.FillSlot();
            var inventoryItem = _inventoryItemFactory.Create().GetComponent<InventoryItem>();
            inventoryItem.transform.SetParent(inventorySlot.transform, false);
            inventoryItem.SetItem(item);
            inventoryItem.UpdatePresentInventory(PresentInventory.Temporary);
            _temporaryInventoryItems.Add(inventoryItem);
        }
    }

    public void MoveItemToTemporaryInventory(InventoryItem inventoryItem)
    {
        var mainInventorySlot = inventoryItem.GetComponentInParent<InventorySlot>();
        var temporaryInventorySlot = _temporaryInventorySlots.FirstOrDefault(x => !x.IsFull);

        if (temporaryInventorySlot == null)
        {
            Debug.Log("Temporary inventory is full. New item can not be added!");

            return;
        }

        inventoryItem.transform.SetParent(temporaryInventorySlot.transform, false);
        _temporaryInventoryItems.Add(inventoryItem);
        temporaryInventorySlot.FillSlot();
        inventoryItem.UpdatePresentInventory(PresentInventory.Temporary);
        _inventoryItems.Remove(inventoryItem);
        mainInventorySlot.EmptySlot();
    }

    public void MoveItemToMainInventory(InventoryItem inventoryItem)
    {
        var acutionInventorySlot = inventoryItem.GetComponentInParent<InventorySlot>();
        var mainInventorySlot = _inventorySlots.First(x => !x.IsFull);

        if (mainInventorySlot == null)
        {
            Debug.Log("Inventory is full. New item can not be added!");

            return;
        }

        inventoryItem.transform.SetParent(mainInventorySlot.transform, false);
        _inventoryItems.Add(inventoryItem);
        mainInventorySlot.FillSlot();
        inventoryItem.UpdatePresentInventory(PresentInventory.Main);
        _temporaryInventoryItems.Remove(inventoryItem);
        acutionInventorySlot.EmptySlot();
    }

    public void DiscardItemFromInventory(InventoryItem inventoryItem)
    {
        var inventorySlot = inventoryItem.GetComponentInParent<InventorySlot>();
        inventorySlot.EmptySlot();
        _inventoryItems.Remove(inventoryItem);
        Destroy(inventoryItem);
    }

    public void CreateMainInventorySlots(int inventorySlotCount)
    {
        for (var i = 0; i < inventorySlotCount; i++)
        {
            var inventorySlot = Instantiate(inventorySlotPrefab, mainInventorySpawnParent)
                .GetComponent<InventorySlot>();
            _inventorySlots.Add(inventorySlot);
        }
    }

    public void CreateTemporaryInventorySlots(int inventorySlotCount)
    {
        for (var i = 0; i < inventorySlotCount; i++)
        {
            var inventorySlot = Instantiate(inventorySlotPrefab, temporaryInventorySpawnParent)
                .GetComponent<InventorySlot>();
            _temporaryInventorySlots.Add(inventorySlot);
        }
    }

    private void GoToHomeScreen()
    {
        UpdateInventories();
        uiManager.ShowScreen(_uiHomeScreen);
    }

    private void GoToAuctionPreparationScreen()
    {
        UpdateInventories();
        uiManager.ShowScreen(_uiGameScreen);
    }

    private void UpdateInventories()
    {
        var items = new List<ItemCardConfig>();
        _inventoryItems.ForEach(x => items.Add(x.ItemConfig));

        _inventoryManager.UpdateInventory(items);
        _inventoryManager.UpdateTemporaryInventory(new());
    }

    public override Task Show()
    {
        CreateMainInventorySlots(_inventoryManager.MainInventorySlotCount);
        CreateTemporaryInventorySlots(_inventoryManager.TemporaryInventorySlotCount);
        LoadItemsToInventory(_inventoryManager.Items);
        LoadItemsToTemporaryInventory(_inventoryManager.TemporaryItems);

        return base.Show();
    }

    public override Task Hide()
    {
        Renew();

        return base.Hide();
    }

    public void Renew()
    {
        _inventorySlots.ForEach(x => Destroy(x.gameObject));
        _inventorySlots.Clear();
        _temporaryInventorySlots.ForEach(x => Destroy(x.gameObject));
        _temporaryInventorySlots.Clear();
        _inventoryItems.ForEach(x => Destroy(x.gameObject));
        _inventoryItems.Clear();
        _temporaryInventoryItems.ForEach(x => Destroy(x.gameObject));
        _temporaryInventoryItems.Clear();
    }
}
