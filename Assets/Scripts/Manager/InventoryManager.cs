using Merchant.Config;
using Merchant.Inventory;
using Merchant.UI.Screen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Merchant.Manager
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private InventoryConfig inventoryConfig;

        private float _burden;

        private List<InventoryItem> _items;
        private List<InventoryItem> _temporaryItems;
        private List<InventoryItem> _auctionItems;

        public ReadOnlyCollection<InventoryItem> Items
            => _items.AsReadOnly();
        public ReadOnlyCollection<InventoryItem> TemporaryItems
            => _temporaryItems.AsReadOnly();
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

        private void Awake()
        {
            _temporaryItems = new List<InventoryItem>();
            _items = new List<InventoryItem>();
            _auctionItems = new List<InventoryItem>();

            // Development purpose
            d_itemCardConfigs.ForEach(x => AddItemToInventory(x));
        }

        public void AddItemToInventory(ItemCardConfig itemConfig)
        {
            var inventoryItem = Items.FirstOrDefault(x => x.ItemConfig == itemConfig);

            if (inventoryItem == null || inventoryItem.IsStackFull())
            {
                inventoryItem = _inventoryItemFactory.Create();
                inventoryItem.SetInventoryItem(itemConfig);
            }

            inventoryItem.IncreaseStack();
            _items.Add(inventoryItem);
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

            inventoryItem.IncreaseStack();
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

            item.ReduceStack(craftComponentCount);
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

        public bool IsTemporaryInventoryFull()
        {
            return _temporaryItems.Count == inventoryConfig.TemporaryInventorySlotCount;
        }

        public bool IsInventoryFull()
        {
            return _items.Count == inventoryConfig.MainInventorySlotCount;
        }

        public bool IsAuctionInventoryFull()
        {
            return _auctionItems.Count == inventoryConfig.AuctionInventorySlotCount;
        }

        #endregion

        #region Debug 

        [SerializeField] List<ItemCardConfig> d_itemCardConfigs;
        private const string d_folderName = "Assets/Configs/Item Card Configs";

        [ContextMenu("Load Item Configs")]
        public void LoadScriptableObjects()
        {
            d_itemCardConfigs.Clear();

            string[] guids = AssetDatabase.FindAssets("t:ItemCardConfig", new string[] { d_folderName });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ItemCardConfig itemCardConfig = AssetDatabase.LoadAssetAtPath<ItemCardConfig>(assetPath);

                if (itemCardConfig != null)
                    d_itemCardConfigs.Add(itemCardConfig);
            }
        }

        #endregion
    }
}
