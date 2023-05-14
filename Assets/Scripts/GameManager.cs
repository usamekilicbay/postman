using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private UIManager _uiManager;
    private InventoryManager _inventoryManager;
    private CurrencyManager _currencyManager;
    private DeckManager _deckManager;
    private AuctionDeckManager _auctionDeckManager;

    [Inject]
    public void Construct(UIManager uiManager,
        InventoryManager inventoryManager,
        CurrencyManager currencyManager,
        DeckManager deckManager,
        AuctionDeckManager auctionDeckManager)
    {
        _uiManager = uiManager;
        _inventoryManager = inventoryManager;
        _currencyManager = currencyManager;
        _deckManager = deckManager;
        _auctionDeckManager = auctionDeckManager;
    }

    public void StartItemCollectRun()
    {
        _deckManager.StartRun();
    }

    public void CompleteItemCollect()
    {

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
