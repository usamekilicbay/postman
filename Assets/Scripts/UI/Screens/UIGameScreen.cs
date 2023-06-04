using Merchant.Common.Abstract;
using Merchant.Inventory;
using Merchant.Manager;
using Merchant.UI.Inventory;
using Merchant.UI.Inventory.Slot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Merchant.UI.Screen
{
    public class UIGameScreen : UIScreenBase, IRenewable
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI remaningCardCountText;
        [SerializeField] private Button completeRunButton;
        [Space(10)]
        [SerializeField] private GameObject inventorySlotPrefab;
        [SerializeField] private Transform inventorySpawnParent;

        private readonly List<UIInventorySlot> _inventorySlots = new();
        private readonly List<UIInventoryItem> _inventoryItems = new();

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

        private void Awake()
        {
            completeRunButton.onClick
                .AddListener(_gameManager.CompleteItemCollect);
        }

        public void UpdateMoneyText(int money)
        {
            moneyText.SetText($"${money}");
        }

        public void UpdateRemaningItemCountText(int remaningCardCount)
        {
            remaningCardCountText.SetText($"Remaining Cards: {remaningCardCount}");
        }

        public void AddItemToInventory(InventoryItem item)
        {
            var inventorySlot = _inventorySlots.FirstOrDefault(x => x.IsEmpty);

            if (inventorySlot == null)
            {
                Debug.Log("Inventory is full. New item can not be added!");

                return;
            }

            //inventorySlot.FillSlot();
            var inventoryItem = _inventoryItemFactory.Create().GetComponent<UIInventoryItem>();
            inventoryItem.transform.SetParent(inventorySlot.transform, false);
            inventoryItem.SetItem(item);
            _inventoryItems.Add(inventoryItem);
        }

        public void DiscardItemFromInventory(UIInventoryItem inventoryItem)
        {
            var inventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            inventorySlot.RemoveItem();
            _inventoryItems.Remove(inventoryItem);
            Destroy(inventoryItem);
        }

        public void CreateInventorySlots(int inventorySlotCount)
        {
            for (var i = 0; i < inventorySlotCount; i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, inventorySpawnParent)
                    .GetComponent<UIInventorySlot>();
                _inventorySlots.Add(inventorySlot);
            }
        }

        public override Task Show()
        {
            var auctionItems = _inventoryManager.AuctionItems;

            if (auctionItems.Any())
            {
                CreateInventorySlots(_inventoryManager.AuctionInventorySlotCount);
                auctionItems.ToList().ForEach(x => AddItemToInventory(x));
            }
            else
                CreateInventorySlots(_inventoryManager.TemporaryInventorySlotCount);


            return base.Show();
        }

        public override Task Hide()
        {
            Renew();

            return base.Hide();
        }

        public void Renew()
        {
            moneyText.SetText($"${0}");

            _inventorySlots.ForEach(x => Destroy(x.gameObject));
            _inventorySlots.Clear();
            _inventoryItems.ForEach(x => Destroy(x.gameObject));
            _inventoryItems.Clear();
        }
    }
}
