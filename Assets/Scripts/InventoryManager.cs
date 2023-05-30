using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    [Inject]
    public void Construct(UIGameScreen uiGameScreen,
        UIAuctionPreparationScreen uiAuctionPreparationScreen)
    {
        _uiGameScreen = uiGameScreen;
        _uiAuctionPreparationScreen = uiAuctionPreparationScreen;
    }

    private void Start()
    {
        _temporaryItems = new List<InventoryItem>();
        _items = new List<InventoryItem>();
        _auctionItems = new List<InventoryItem>();
    }

    public bool CollectItem(ItemCardConfig item)
    {
        //if (_burden >= inventoryConfig.CarryCapacity
        //    && Random.Range(0f, 1f) < inventoryConfig.ItemDiscardProbability)
        //{
        //    var selectedItem = ScriptableObject.CreateInstance<ItemCardConfig>();

        //    do
        //    {
        //        var randomIndex = Random.Range(0, temporaryItems.Count);
        //        selectedItem = temporaryItems[randomIndex];

        //    } while (_burden - selectedItem.Weight + item.Weight <= inventoryConfig.CarryCapacity);

        //    temporaryItems.Remove(selectedItem);

        //    return false;
        //}

        if (_temporaryItems.Count == inventoryConfig.TemporaryInventorySlotCount)
        {
            Debug.Log("Temporary inventory is full, new item can not be collected!");

            return false;
        }

        // TODO: Check if item exist in the inventory if not create new one

        _temporaryItems.Add(item);
        _uiGameScreen.AddItemToInventory(item);
        _burden += item.GetTotalWeight();
        return true;
    }

    public void DiscardItem(InventoryItem item)
    {
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
