using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Inventory
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

            moveItemButton.onClick.AddListener(SendItemToInventoryManager);
        }

        public void SetItem(InventoryItem item)
        {
            InventoryItem = item;
            image.sprite = InventoryItem.ItemConfig.Artwork;
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
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            if (eventData.pointerEnter == null)
            {
                transform.SetParent(_originalParent);
                _rectTransform.anchoredPosition = Vector2.zero;

                return;
            }

            if (eventData.pointerEnter.TryGetComponent<UIInventorySlot>(out var destinationInventorySlot))
                destinationInventorySlot.AddItem(this, _rectTransform);
        }


        public class Factory : PlaceholderFactory<UIInventoryItem>
        {

        }
    }
}
