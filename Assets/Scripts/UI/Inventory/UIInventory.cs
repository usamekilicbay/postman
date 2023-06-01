using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace UI.Inventory
{
    public class UIInventory : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject inventorySlotPrefab;
        [Space(10)]
        [Header("Inventories")]
        [SerializeField] private Transform inventorySpawnParent;

        private List<UIInventorySlot> _inventorySlots = new();
        private List<UIInventoryItem> _inventoryItems = new();

        private GameManager _gameManager;
        private InventoryManager _inventoryManager;
        private UIInventoryItem.Factory _inventoryItemFactory;

        [Inject]
        public void Construct(GameManager gameManager,
            InventoryManager inventoryManager,
            UIInventoryItem.Factory inventoryItemFactory)
        {
            _gameManager = gameManager;
            _inventoryManager = inventoryManager;
            _inventoryItemFactory = inventoryItemFactory;
        }

        private void OnEnable()
        {
            Show();
        }

        private void OnDisable()
        {
            Renew();
        }

        public void LoadItemsToInventory(IReadOnlyList<InventoryItem> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                InventoryItem item = items[i];
                var inventorySlot = _inventorySlots[i];
                inventorySlot.FillSlot();
                var inventoryItem = _inventoryItemFactory.Create().GetComponent<UIInventoryItem>();
                inventoryItem.transform.SetParent(inventorySlot.transform, false);
                inventoryItem.SetItem(item);
                _inventoryItems.Add(inventoryItem);
            }
        }

        public void MoveItemToAuctionInventory(UIInventoryItem inventoryItem)
        {
            var mainInventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            var auctionInventorySlot = _auctionInventorySlots.FirstOrDefault(x => !x.IsFull);

            if (auctionInventorySlot == null)
            {
                Debug.Log("Auction inventory is full. New item can not be added!");

                return;
            }

            inventoryItem.transform.SetParent(auctionInventorySlot.transform, false);
            _auctionUIInventoryItems.Add(inventoryItem);
            auctionInventorySlot.FillSlot();
            inventoryItem.UpdatePresentInventory(PresentInventory.Auction);
            _inventoryItems.Remove(inventoryItem);
            mainInventorySlot.EmptySlot();
        }

        public void MoveItemToMainInventory(UIInventoryItem inventoryItem)
        {
            var acutionInventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            var mainInventorySlot = _inventorySlots.First(x => !x.IsFull);
            inventoryItem.transform.SetParent(mainInventorySlot.transform, false);
            _inventoryItems.Add(inventoryItem);
            mainInventorySlot.FillSlot();
            inventoryItem.UpdatePresentInventory(PresentInventory.Main);
            _auctionUIInventoryItems.Remove(inventoryItem);
            acutionInventorySlot.EmptySlot();
        }

        public void DiscardItemFromInventory(UIInventoryItem inventoryItem)
        {
            var inventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            inventorySlot.EmptySlot();
            _inventoryItems.Remove(inventoryItem);
            Destroy(inventoryItem);
        }

        public void CreateInventorySlots(int slotCount)
        {
            for (var i = 0; i < slotCount; i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, inventorySpawnParent)
                    .GetComponent<UIInventorySlot>();
                _inventorySlots.Add(inventorySlot);
            }
        }

        public virtual void StartAuction()
        {
            var items = new List<InventoryItem>();
            _inventoryItems.ForEach(x => items.Add(x.InventoryItem));
            
            _inventoryManager.UpdateInventory(items);
        }

         public void Show()
        {
            CreateInventorySlots(_inventoryManager.MainInventorySlotCount);
            LoadItemsToInventory(_inventoryManager.Items);
        }

        public void Renew()
        {
            _inventorySlots.ForEach(x => Destroy(x.gameObject));
            _inventorySlots.Clear();
            _inventoryItems.ForEach(x => Destroy(x.gameObject));
            _inventoryItems.Clear();
        }
    }
}
