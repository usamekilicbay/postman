using Merchant.Inventory;
using Merchant.Manager;
using Merchant.UI.Inventory.Slot;
using Merchant.UI.Screen;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Merchant.UI.Inventory
{
    public enum PresentInventory
    {
        Main,
        Auction,
        Temporary
    }

    [RequireComponent(typeof(RectTransform), typeof(Image), typeof(CanvasGroup))]
    public class UIInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Button moveItemButton;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI stackText;

        public UIInventorySlot CurrentSlot { get; private set; }
        public PresentInventory PresentInventory { get; private set; }

        public InventoryItem InventoryItem { get; private set; }

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Transform _originalParent;
        private Vector2 _originalPosition;

        private UIManagerBase _uiManager;
        private UIGameScreen _uiGameScreen;
        private UIAuctionPreparationScreen _uiAuctionPreparationScreen;
        private UIGameResultScreen _uiGameResultScreen;

        [Inject]
        public void Construct(UIManagerBase uiManager,
            UIGameScreen uiGameScreen,
            UIAuctionPreparationScreen uiAuctionPreparationScreen,
            UIGameResultScreen uiGameResultScreen)
        {
            _uiManager = uiManager;
            _uiGameScreen = uiGameScreen;
            _uiAuctionPreparationScreen = uiAuctionPreparationScreen;
            _uiGameResultScreen = uiGameResultScreen;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();

            //moveItemButton.onClick.AddListener(SendItemToInventoryManager);
        }

        public void SetItem(InventoryItem item)
        {
            InventoryItem = item;
            image.sprite = InventoryItem.ItemConfig.Artwork;
            stackText.SetText(item.StackCount.ToString());
        }

        public void SendItemToInventoryManager()
        {
            switch (PresentInventory)
            {
                case PresentInventory.Main:
                    if (_uiManager.GetActiveUIScreen() == _uiAuctionPreparationScreen)
                        _uiAuctionPreparationScreen.MoveItemToAuctionInventory(this);
                    else
                        _uiGameResultScreen.MoveItemToTemporaryInventory(this);
                    break;
                case PresentInventory.Auction:
                    _uiAuctionPreparationScreen.MoveItemToMainInventory(this);
                    break;
                case PresentInventory.Temporary:
                    _uiGameResultScreen.MoveItemToMainInventory(this);
                    break;
            }
        }

        // TODO: Add Discard button and bind this method to it
        public void DiscardItem()
        {
            switch (PresentInventory)
            {
                case PresentInventory.Main:
                    _uiAuctionPreparationScreen.DiscardItemFromInventory(this);
                    break;
                case PresentInventory.Auction:
                    break;
                case PresentInventory.Temporary:
                    _uiGameScreen.DiscardItemFromInventory(this);
                    break;
            }
        }

        public void UpdateSlot(UIInventorySlot slot)
        {
            CurrentSlot = slot;
            transform.SetParent(CurrentSlot.transform);
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
            _rectTransform.localScale = Vector3.one;
        }

        public void UpdatePresentInventory(PresentInventory presentInventory)
        {
            PresentInventory = presentInventory;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            _originalPosition = transform.position;
            transform.SetParent(transform.root);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / transform.root.localScale.x;

            Debug.Log(eventData.pointerEnter?.name);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            if (eventData.pointerEnter != null)
            {
                if (eventData.pointerEnter.TryGetComponent<UIInventoryItem>(out var destinationItem))
                    SwapItems(destinationItem);
                else if (eventData.pointerEnter.TryGetComponent<UIInventorySlot>(out var destinationInventorySlot))
                {
                    if (destinationInventorySlot == CurrentSlot)
                    {
                        Debug.Log("Same slot");
                        ReturnToCurrentSlot();
                        return;
                    }

                    MoveToSlot(destinationInventorySlot);
                }

                return;
            }

            Debug.Log("Can't drop");
            ReturnToCurrentSlot();
        }

        private void SwapItems(UIInventoryItem swappingItem)
        {
            Debug.Log("Swap");
            var destinationSlot = swappingItem.CurrentSlot;
            CurrentSlot.RemoveItem();
            destinationSlot.RemoveItem();
            CurrentSlot.AddItem(swappingItem);
            destinationSlot.AddItem(this);
        }

        private void MoveToSlot(UIInventorySlot destinationSlot)
        {
            Debug.Log("Move");
            CurrentSlot.RemoveItem();
            destinationSlot.AddItem(this);
        }

        private void ReturnToCurrentSlot()
        {
            transform.SetParent(_originalParent);
            _rectTransform.anchoredPosition = Vector2.zero;
        }

        public class Factory : PlaceholderFactory<UIInventoryItem>
        {
            ///
        }
    }
}
