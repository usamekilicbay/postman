using DG.Tweening;
using Merchant.Manager;
using TMPro;
using UnityEngine;
using Zenject;

namespace Merchant.Card
{
    public abstract class CardBase : MonoBehaviour
    {
        [SerializeField] private GameObject frontSide;
        [SerializeField] private GameObject backSide;
        [SerializeField] private SpriteRenderer cloakRenderer;
        [Space(10)]
        [Header("Front Details")]
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected TextMeshProUGUI frontSideNameText;
        [Header("Back Details")]
        [SerializeField] protected TextMeshProUGUI backSideNameText;
        [Space(10)]
        [SerializeField] private Transform _pivotTransform;

        private bool _isBackSideShown;
        private Vector3 _defaultScale;

        public bool IsUsed { get; protected set; }

        #region Dependency Injection

        protected DeckManager deckManager;
        protected AuctionDeckManager auctionDeckManager;
        protected InventoryManager inventoryManager;
        protected ICurrencyManager currencyManager;

        [Inject]
        public void Construct(CardFacade cardFacade)
        {
            deckManager = cardFacade.DeckManager;
            auctionDeckManager = cardFacade.AuctionDeckManager;
            inventoryManager = cardFacade.InventoryManager;
            currencyManager = cardFacade.CurrencyManager;
        }

        #endregion

        #region Unity

        private void Awake()
        {
            Setup();
        }

        private void OnMouseDrag()
        {
            //_pivotTransform.rotation = 
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                FlipCard();
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                SwipeRight();
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                SwipeLeft();
        }

        #endregion

        private void Setup()
        {
            frontSide.SetActive(true);
            backSide.SetActive(false);
            backSide.transform.rotation = Quaternion.Euler(Vector3.up * 180);
            _defaultScale = transform.localScale;
            transform.localScale = _defaultScale * 0.7f;
            transform.DOScale(_defaultScale, 0.5f);
            var cloakColor = cloakRenderer.color;
            cloakColor.a = 0;
            cloakRenderer.color = cloakColor;
        }

        private void FlipCard()
        {
            transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 90, 0.5f)
                .OnComplete(UpdateShownSide);
        }

        private void UpdateShownSide()
        {
            if (_isBackSideShown)
            {
                frontSide.SetActive(true);
                backSide.SetActive(false);
                _isBackSideShown = false;
            }
            else
            {
                frontSide.SetActive(false);
                backSide.SetActive(true);
                _isBackSideShown = true;
            }

            transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 90, 0.5f);
        }

        protected virtual void SwipeRight()
        {
            VanishCard();
        }
        
        protected virtual void SwipeLeft()
        {
            Debug.Log("Passed");
            deckManager.SpawnCard();

            transform.DOMoveX(-5f, 0.5f)
                 .OnComplete(VanishCard);
        }

        private void VanishCard()
        {
            cloakRenderer.DOFade(1, 0.5f)
                .OnComplete(() => Destroy(gameObject));
        }

        //private void LookAtCursor()
        //{
        //    float angle = Mathf.Atan2(CursorTracker.Direction.y, CursorTracker.Direction.x) * Mathf.Rad2Deg;

        //    Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);

        //    _skierArtTransform.rotation = rotation;
        //}
    }
}
