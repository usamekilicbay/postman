using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Zenject;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryConfig inventoryConfig;

    private float _burden;

    private List<ItemCardConfig> _temporaryItems;
    private List<ItemCardConfig> _items;
    private List<ItemCardConfig> _auctionItems;

    public ReadOnlyCollection<ItemCardConfig> Items
        => _items.AsReadOnly();
    public ReadOnlyCollection<ItemCardConfig> AuctionItems
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
        _temporaryItems = new List<ItemCardConfig>();
        _items = new List<ItemCardConfig>();
        _auctionItems = new List<ItemCardConfig>();
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

        _temporaryItems.Add(item);
        _uiGameScreen.AddItemToInventory(item);
        _burden += item.Weight;
        return true;
    }

    public void DiscardItem(ItemCardConfig item)
    {
        _auctionItems.Remove(item);
    }

    public void StartRun()
    {
        //GenerateTemporaryInventorySlots();
    }

    public void CompleteItemCollectRun()
    {
        _items.AddRange(_temporaryItems);
        _temporaryItems.Clear();
    }

    public void CompleteAuctionRun()
    {
        _auctionItems.Clear();
    }

    public void UpdateInventories(List<ItemCardConfig> items, List<ItemCardConfig> auctionItems)
    {
        _items = items;
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
