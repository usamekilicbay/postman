using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIAuctionPreparationScreen : UIScreenBase
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Button startAuctionButton;
    [Space(10)]
    [Header("Prefabs")]
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private Transform inventorySpawnParentTransform;

    private UIHomeScreen _uiHomeScreen;
    private UIGameScreen _uiGameScreen;

    [Inject]
    public void Construct(UIHomeScreen homeScreen,
        UIGameScreen gameScreen)
    {
        _uiGameScreen = gameScreen;
        _uiHomeScreen = homeScreen;
    }

    private void Awake()
    {
        homeButton.onClick
            .AddListener(() => uiManager.ShowScreen(_uiHomeScreen));

        startAuctionButton.onClick
            .AddListener(() => uiManager.ShowScreen(_uiGameScreen));
    }

    public void AddItemToInventory(IReadOnlyList<ItemCardConfig> items)
    {
        foreach (var item in items)
        {
            var inventorySlot = Instantiate(inventorySlotPrefab, inventorySpawnParentTransform);
            var inventoryItem = Instantiate(inventoryItemPrefab, inventorySlot.transform);
            inventoryItem.GetComponentInChildren<Image>().sprite = item.Artwork;
        }
    }
}
