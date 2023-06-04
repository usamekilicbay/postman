using Merchant.Common.Abstract;
using Merchant.Inventory;
using Merchant.Manager;
using Merchant.UI.Inventory;
using Merchant.UI.Inventory.Slot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Merchant.UI.Screen
{
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

        private List<UIInventorySlot> _inventorySlots = new();
        private List<UIInventorySlot> _temporaryInventorySlots = new();
        private List<UIInventoryItem> _inventoryItems = new();
        private List<UIInventoryItem> _temporaryUIInventoryItems = new();

        private GameManager _gameManager;
        private InventoryManager _inventoryManager;
        private UIHomeScreen _uiHomeScreen;
        private UIGameScreen _uiGameScreen;
        private UIInventoryItem.Factory _inventoryItemFactory;

        [Inject]
        public void Construct(GameManager gameManager,
            InventoryManager inventoryManager,
            UIHomeScreen homeScreen,
            UIGameScreen gameScreen,
            UIInventoryItem.Factory inventoryItemFactory)
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

        public void LoadItemsToInventory(IReadOnlyList<InventoryItem> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                InventoryItem item = items[i];
                var inventorySlot = _inventorySlots[i];
                //inventorySlot.FillSlot();
                var inventoryItem = _inventoryItemFactory.Create().GetComponent<UIInventoryItem>();
                inventoryItem.transform.SetParent(inventorySlot.transform, false);
                inventoryItem.SetItem(item);
                inventoryItem.UpdatePresentInventory(PresentInventory.Main);
                _inventoryItems.Add(inventoryItem);
            }
        }

        public void LoadItemsToTemporaryInventory(IReadOnlyList<InventoryItem> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                InventoryItem item = items[i];
                var inventorySlot = _temporaryInventorySlots[i];
                //inventorySlot.FillSlot();
                var inventoryItem = _inventoryItemFactory.Create().GetComponent<UIInventoryItem>();
                inventoryItem.transform.SetParent(inventorySlot.transform, false);
                inventoryItem.SetItem(item);
                inventoryItem.UpdatePresentInventory(PresentInventory.Temporary);
                _temporaryUIInventoryItems.Add(inventoryItem);
            }
        }

        public void MoveItemToTemporaryInventory(UIInventoryItem inventoryItem)
        {
            var mainInventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            var temporaryInventorySlot = _temporaryInventorySlots.FirstOrDefault(x => x.IsEmpty);

            if (temporaryInventorySlot == null)
            {
                Debug.Log("Temporary inventory is full. New item can not be added!");

                return;
            }

            inventoryItem.transform.SetParent(temporaryInventorySlot.transform, false);
            _temporaryUIInventoryItems.Add(inventoryItem);
            //temporaryInventorySlot.FillSlot();
            inventoryItem.UpdatePresentInventory(PresentInventory.Temporary);
            _inventoryItems.Remove(inventoryItem);
            mainInventorySlot.RemoveItem();
        }

        public void MoveItemToMainInventory(UIInventoryItem uiInventoryItem)
        {
            var acutionInventorySlot = uiInventoryItem.GetComponentInParent<UIInventorySlot>();
            var mainInventorySlot = _inventorySlots.First(x => x.IsEmpty);

            if (mainInventorySlot == null)
            {
                Debug.Log("Inventory is full. New item can not be added!");

                return;
            }

            uiInventoryItem.transform.SetParent(mainInventorySlot.transform, false);
            _inventoryItems.Add(uiInventoryItem);
            //mainInventorySlot.FillSlot();
            uiInventoryItem.UpdatePresentInventory(PresentInventory.Main);
            _temporaryUIInventoryItems.Remove(uiInventoryItem);
            acutionInventorySlot.RemoveItem();
        }

        public void DiscardItemFromInventory(UIInventoryItem inventoryItem)
        {
            var inventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            inventorySlot.RemoveItem();
            _inventoryItems.Remove(inventoryItem);
            Destroy(inventoryItem);
        }

        public void CreateMainInventorySlots(int inventorySlotCount)
        {
            for (var i = 0; i < inventorySlotCount; i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, mainInventorySpawnParent)
                    .GetComponent<UIInventorySlot>();
                _inventorySlots.Add(inventorySlot);
            }
        }

        public void CreateTemporaryInventorySlots(int inventorySlotCount)
        {
            for (var i = 0; i < inventorySlotCount; i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, temporaryInventorySpawnParent)
                    .GetComponent<UIInventorySlot>();
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
            var items = new List<InventoryItem>();
            _inventoryItems.ForEach(x => items.Add(x.InventoryItem));

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
            _temporaryUIInventoryItems.ForEach(x => Destroy(x.gameObject));
            _temporaryUIInventoryItems.Clear();
        }
    }
}
