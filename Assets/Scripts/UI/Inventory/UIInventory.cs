using Merchant.Inventory;
using Merchant.Manager;
using Merchant.UI.Inventory.Slot;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Merchant.UI.Inventory
{
    public class UIInventory : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject inventorySlotPrefab;
        [Space(10)]
        [Header("Inventories")]
        [SerializeField] private Transform inventorySpawnParent;

        private List<UIInventorySlot> _inventorySlots = new();
        private List<UIInventoryItem> _uiInventoryItems = new();

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

        public void LoadItemsToInventory(IReadOnlyList<InventoryItem> inventoryItems)
        {
            for (var i = 0; i < inventoryItems.Count; i++)
            {
                InventoryItem inventoryItem = inventoryItems[i];
                var uiInventoryItem = _inventoryItemFactory.Create().GetComponent<UIInventoryItem>();
                uiInventoryItem.SetItem(inventoryItem);
                var uiInventorySlot = _inventorySlots[i];
                uiInventorySlot.AddItem(uiInventoryItem);
                _uiInventoryItems.Add(uiInventoryItem);
            }
        }

        public void MoveItemToMainInventory(InventoryItem inventoryItem)
        {
            //var acutionInventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            //var mainInventorySlot = _inventorySlots.First(x => x.IsEmpty);
            //inventoryItem.transform.SetParent(mainInventorySlot.transform, false);
            //_uiInventoryItems.Add(inventoryItem);
            ////mainInventorySlot.FillSlot();
            //inventoryItem.UpdatePresentInventory(PresentInventory.Main);
            //acutionInventorySlot.EmptySlot();
        }

        public void DiscardItemFromInventory(UIInventoryItem inventoryItem)
        {
            var inventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            inventorySlot.RemoveItem();
            _uiInventoryItems.Remove(inventoryItem);
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
            _uiInventoryItems.ForEach(x => items.Add(x.InventoryItem));
            
            //_inventoryManager.UpdateInventory(items);
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
            _uiInventoryItems.ForEach(x => Destroy(x.gameObject));
            _uiInventoryItems.Clear();
        }
    }
}
