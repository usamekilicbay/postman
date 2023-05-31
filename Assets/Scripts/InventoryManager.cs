using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Zenject;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryConfig inventoryConfig;

    private float _burden;

    private List<InventoryItem> _temporaryItems;
    private List<InventoryItem> _items;
    private List<InventoryItem> _auctionItems;

    public ReadOnlyCollection<InventoryItem> TemporaryItems
        => _temporaryItems.AsReadOnly();
    public ReadOnlyCollection<InventoryItem> Items
        => _items.AsReadOnly();
    public ReadOnlyCollection<InventoryItem> AuctionItems
        => _auctionItems.AsReadOnly();

    public int TemporaryInventorySlotCount { get => inventoryConfig.TemporaryInventorySlotCount; }
    public int MainInventorySlotCount { get => inventoryConfig.MainInventorySlotCount; }
    public int AuctionInventorySlotCount { get => inventoryConfig.AuctionInventorySlotCount; }


    private UIGameScreen _uiGameScreen;
    private UIAuctionPreparationScreen _uiAuctionPreparationScreen;
    private InventoryItem.Factory _inventoryItemFactory;

    [Inject]
    public void Construct(UIGameScreen uiGameScreen,
        UIAuctionPreparationScreen uiAuctionPreparationScreen,
        InventoryItem.Factory inventoryItemFactory)
    {
        _uiGameScreen = uiGameScreen;
        _uiAuctionPreparationScreen = uiAuctionPreparationScreen;
        _inventoryItemFactory = inventoryItemFactory;
    }

    private void Start()
    {
        _temporaryItems = new List<InventoryItem>();
        _items = new List<InventoryItem>();
        _auctionItems = new List<InventoryItem>();
    }

    public bool CollectItem(ItemCardConfig item)
    {

        // TODO: If drop probability will be added to the real game, this code has to be activated
        // and DropRandomItem method must be updated.
        //if (_burden >= inventoryConfig.CarryCapacity
        //    && Random.Range(0f, 1f) < inventoryConfig.ItemDiscardProbability)
        //{
        //    DropRandomItem(item);

        //    return false;
        //}

        //if (_temporaryItems.Count == inventoryConfig.TemporaryInventorySlotCount)
        //{
        //    Debug.Log("Temporary inventory is full, new item can not be collected!");

        //    return false;
        //}

        if (_burden >= inventoryConfig.CarryCapacity)
            Debug.LogWarning("Burden has reached the maximum weight, you can not collect more item.");

        var inventoryItem = _temporaryItems.FirstOrDefault(x => x.ItemConfig == item);

        if (inventoryItem == null || inventoryItem.IsStackFull())
        {
            inventoryItem = _inventoryItemFactory.Create();
            inventoryItem.SetInventoryItem(item);
        }

        inventoryItem.AddItemToStack();
        _temporaryItems.Add(inventoryItem);
        _uiGameScreen.AddItemToInventory(inventoryItem);
        _burden += inventoryItem.GetTotalWeight();

        return true;
    }

    public bool HasEnoughComponentToCraft(string craftComponentId, int amount) 
        => GetCraftComponentAmountById(craftComponentId) >= amount;

    // TODO: Update this method by reducing amount from another slot if 1 slot is not enough.
    public void DiscardCraftComponent(string craftComponentId, int craftComponentCount)
    {
        var item = GetCraftComponentById(craftComponentId);

        item.RemoveItemFromStack(craftComponentCount);
    }

    public void DiscardAuctionItem(ItemCardConfig itemConfig)
    {
        var item = _auctionItems.First(x => x.ItemConfig.Id == itemConfig.Id);
        _auctionItems.Remove(item);
    }

    public void StartRun()
    {
        //GenerateTemporaryInventorySlots();
    }

    public void AddCollectedItemsToInventory()
    {
        _items.AddRange(_temporaryItems);
        _temporaryItems.Clear();
    }

    public void CompleteAuctionRun()
    {
        _auctionItems.Clear();
    }

    public void UpdateTemporaryInventory(List<InventoryItem> items)
    {
        _temporaryItems = items;
    }

    public void UpdateInventory(List<InventoryItem> items)
    {
        _items = items;
    }

    public void UpdateAuctionInventory(List<InventoryItem> auctionItems)
    {
        _auctionItems = auctionItems;
    }

    //private void GenerateTemporaryInventorySlots()
    //{
    //    _uiGameScreen.CreateInventorySlots(inventoryConfig.TemporaryInventorySlotCount);
    //}

    //private void GenerateMainInventorySlots()
    //{
    //    _uiAuctionPreparationScreen.CreateMainInventorySlots(inventoryConfig.MainInventorySlotCount);
    //}

    //private void GenerateAuctionInventorySlots()
    //{
    //    _uiAuctionPreparationScreen.CreateAuctionInventorySlots(inventoryConfig.AuctionInventorySlotCount);
    //}

    private void DropRandomItem(ItemCardConfig item)
    {
        var selectedItem = new InventoryItem();

        do
        {
            var randomIndex = Random.Range(0, _temporaryItems.Count);
            selectedItem = _temporaryItems[randomIndex];

        } while (_burden - selectedItem.ItemConfig.Weight + item.Weight <= inventoryConfig.CarryCapacity);

        _temporaryItems.Remove(selectedItem);
    }

    private InventoryItem GetCraftComponentById(string craftComponentId)
       => _items.First(x => x.ItemConfig.Id == craftComponentId);

    private int GetCraftComponentAmountById(string craftComponentId)
    {
        return _items.Where(x => x.ItemConfig.Id == craftComponentId)
            .Sum(x => x.StackCount);
    }

    #region Exposed

    public bool IsTemporaryItemsInventoryFull()
    {
        return _temporaryItems.Count == inventoryConfig.TemporaryInventorySlotCount;
    }

    public bool IsItemsInventoryFull()
    {
        return _items.Count == inventoryConfig.MainInventorySlotCount;
    }

    public bool IsAuctionItemsInventoryFull()
    {
        return _auctionItems.Count == inventoryConfig.AuctionInventorySlotCount;
    }

    #endregion
}
