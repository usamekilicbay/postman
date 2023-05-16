using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private UIManagerBase _uiManager;
    private InventoryManager _inventoryManager;
    private CurrencyManager _currencyManager;
    private DeckManager _deckManager;
    private AuctionDeckManager _auctionDeckManager;
    private UIHomeScreen _uiHomeScreen;
    private UIGameScreen _uiGameScreen;
    private UIAuctionPreparationScreen _uiAuctionPreparationScreen;

    [Inject]
    public void Construct(UIManagerBase uiManager,
        InventoryManager inventoryManager,
        CurrencyManager currencyManager,
        DeckManager deckManager,
        AuctionDeckManager auctionDeckManager,
        UIHomeScreen uiHomeScreen,
        UIGameScreen uiGameScreen,
        UIAuctionPreparationScreen uiAuctionPreparationScreen)
    {
        _uiManager = uiManager;
        _inventoryManager = inventoryManager;
        _currencyManager = currencyManager;
        _deckManager = deckManager;
        _auctionDeckManager = auctionDeckManager;
        _uiHomeScreen = uiHomeScreen;
        _uiGameScreen = uiGameScreen;
        _uiAuctionPreparationScreen = uiAuctionPreparationScreen;
    }

    public void StartItemCollectRun()
    {
        _deckManager.StartRun();
        _inventoryManager.StartRun();
    }

    public void CompleteItemCollect()
    {
        _inventoryManager.CompleteRun();
        _uiManager.ShowScreen(_uiHomeScreen);
    }

    public void StartAuction(List<ItemCardConfig> items, List<ItemCardConfig> auctionItems)
    {
        _inventoryManager.UpdateInventories(items, auctionItems);
        _auctionDeckManager.StartRun();
    }

    public void CompleteAuction()
    {

    }
}
