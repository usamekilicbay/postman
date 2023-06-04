using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Merchant.Common.Abstract;
using Merchant.Inventory;
using Merchant.Manager;
using Merchant.UI.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Merchant.UI.Screen
{
    public class UIAuctionPreparationScreen : UIScreenBase, IRenewable
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

        private List<UIInventorySlot> _inventorySlots = new();
        private List<UIInventorySlot> _auctionInventorySlots = new();
        private List<UIInventoryItem> _inventoryItems = new();
        private List<UIInventoryItem> _auctionUIInventoryItems = new();

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
                .AddListener(() => uiManager.ShowScreen(_uiHomeScreen));

            startAuctionButton.onClick
                .AddListener(StartAuction);
        }

        public void LoadItemsToInventory(IReadOnlyList<InventoryItem> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                InventoryItem item = items[i];
                var inventorySlot = _inventorySlots[i];
                //inventorySlot.AddItem(item);
                var inventoryItem = _inventoryItemFactory.Create().GetComponent<UIInventoryItem>();
                inventoryItem.transform.SetParent(inventorySlot.transform, false);
                _inventoryItems.Add(inventoryItem);
            }
        }

        public void MoveItemToAuctionInventory(UIInventoryItem inventoryItem)
        {
            var mainInventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            var auctionInventorySlot = _auctionInventorySlots.FirstOrDefault(x => x.IsEmpty);

            if (auctionInventorySlot == null)
            {
                Debug.Log("Auction inventory is full. New item can not be added!");

                return;
            }

            inventoryItem.transform.SetParent(auctionInventorySlot.transform, false);
            _auctionUIInventoryItems.Add(inventoryItem);
            //auctionInventorySlot.AddItem(inventoryItem);
            inventoryItem.UpdatePresentInventory(PresentInventory.Auction);
            _inventoryItems.Remove(inventoryItem);
            mainInventorySlot.EmptySlot();
        }

        public void MoveItemToMainInventory(UIInventoryItem inventoryItem)
        {
            var acutionInventorySlot = inventoryItem.GetComponentInParent<UIInventorySlot>();
            var mainInventorySlot = _inventorySlots.First(x => x.IsEmpty);
            inventoryItem.transform.SetParent(mainInventorySlot.transform, false);
            _inventoryItems.Add(inventoryItem);
            //mainInventorySlot.FillSlot();
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

        public void CreateMainInventorySlots(int inventorySlotCount)
        {
            for (var i = 0; i < inventorySlotCount; i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, mainInventorySpawnParent)
                    .GetComponent<UIInventorySlot>();
                _inventorySlots.Add(inventorySlot);
            }
        }

        public void CreateAuctionInventorySlots(int inventorySlotCount)
        {
            for (var i = 0; i < inventorySlotCount; i++)
            {
                var inventorySlot = Instantiate(inventorySlotPrefab, auctionInventorySpawnParent)
                    .GetComponent<UIInventorySlot>();
                _auctionInventorySlots.Add(inventorySlot);
            }
        }

        public void StartAuction()
        {
            var items = new List<InventoryItem>();
            _inventoryItems.ForEach(x => items.Add(x.InventoryItem));
            var auctionItems = new List<InventoryItem>();
            _auctionUIInventoryItems.ForEach(x => auctionItems.Add(x.InventoryItem));

            if (auctionItems.Count == 0)
            {
                Debug.Log("You can't start auction without items.");
                return;
            }

            //_gameManager.StartAuction(items, auctionItems);
            uiManager.ShowScreen(_uiGameScreen);
        }

        public override Task Show()
        {
            CreateMainInventorySlots(_inventoryManager.MainInventorySlotCount);
            CreateAuctionInventorySlots(_inventoryManager.AuctionInventorySlotCount);
            //LoadItemsToInventory(_inventoryManager.Items);

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
            _auctionInventorySlots.ForEach(x => Destroy(x.gameObject));
            _auctionInventorySlots.Clear();
            _inventoryItems.ForEach(x => Destroy(x.gameObject));
            _inventoryItems.Clear();
            _auctionUIInventoryItems.ForEach(x => Destroy(x.gameObject));
            _auctionUIInventoryItems.Clear();
        }
    }
}
