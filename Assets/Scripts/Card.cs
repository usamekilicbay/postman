using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

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

    private DeckManager _deckManager;
    private InventoryManager _inventoryManager;

    private void OnMouseDrag()
    {
        //_pivotTransform.rotation = 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 90, 0.5f)
                .OnStepComplete(() =>
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

                    transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 90, 0.5f)
                    .OnComplete(() => transform.DORotate(Vector3.zero, 0f));
                });
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.DOMoveX(5f, 0.5f)
                .OnComplete(CollectCard);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.DOMoveX(-5f, 0.5f)
                .OnComplete(PassCard);
        }
    }

    private void CollectCard()
    {
        Debug.Log("Collected");
        _inventoryManager.CollectItem(_itemCardConfig);
        _deckManager.SpawnCard();
        Destroy(gameObject);
    }

    private void PassCard()
    {
        Debug.Log("Passed");
        // TODO: Spawn Next Card
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
}
