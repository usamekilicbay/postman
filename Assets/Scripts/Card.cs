using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Zenject;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject backSide;
    [Space(10)]
    [Header("Front Side")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI frontSideNameText;
    [Header("Back Side")]
    [SerializeField] private TextMeshProUGUI backSideNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI weightText;

    [SerializeField] private Transform _pivotTransform;

    private ItemCardConfig _itemCardConfig;

    private bool _isBackSideShown;
    private Vector3 _defaultScale;

    #region Dependency Injection

    private DeckManager _deckManager;
    private InventoryManager _inventoryManager;
    private CurrencyManager _currencyManager;

    [Inject]
    public void Construct(CardFacade cardFacade)
    {
        _deckManager = cardFacade.DeckManager;
        _inventoryManager = cardFacade.InventoryManager;
        _currencyManager = cardFacade.CurrencyManager;
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
            CollectCard();
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            PassCard();
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

    private async void CollectCard()
    {
        var isNotEnoughMoney = !_currencyManager.BuyItem(_itemCardConfig.Price);

        if (isNotEnoughMoney)
        {
            Debug.Log("Can not be buyed");
            return;
        }

        Debug.Log("Collected");

        _deckManager.SpawnCard();

        await transform.DOMoveX(5f, 0.5f)
             .AsyncWaitForKill();

        _inventoryManager.CollectItem(_itemCardConfig);
        Destroy(gameObject);
    }

    private async void PassCard()
    {
        Debug.Log("Passed");
        _deckManager.SpawnCard();

        await transform.DOMoveX(-5f, 0.5f)
             .AsyncWaitForKill();

        Destroy(gameObject);
    }

    //private void LookAtCursor()
    //{
    //    float angle = Mathf.Atan2(CursorTracker.Direction.y, CursorTracker.Direction.x) * Mathf.Rad2Deg;

    //    Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);

    //    _skierArtTransform.rotation = rotation;
    //}

    public void SetConfigs(ItemCardConfig config)
    {
        _itemCardConfig = config;

        spriteRenderer.sprite = config.Artwork;
        frontSideNameText.SetText(config.Name);
        backSideNameText.SetText(config.Name);
        descriptionText.SetText(config.Description);
        priceText.SetText($"${config.Price}");
        rarityText.SetText(Enum.GetName(typeof(Rarity), config.Rarity));
        weightText.SetText(config.Weight.ToString("F1"));
    }

    public class Factory : PlaceholderFactory<Card>
    {
    }
}
