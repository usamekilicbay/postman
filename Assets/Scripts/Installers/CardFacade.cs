using Zenject;

public class CardFacade
{
    public DeckManager DeckManager { get; private set; }
    public AuctionDeckManager AuctionDeckManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }
    public ICurrencyManager CurrencyManager { get; private set; }

    [Inject]
    public void Construct(DeckManager deckManager, 
        AuctionDeckManager auctionDeckManager,
        InventoryManager inventoryManager,
        ICurrencyManager currencyManager)
    {
        DeckManager = deckManager;
        AuctionDeckManager = auctionDeckManager;
        InventoryManager = inventoryManager;
        CurrencyManager = currencyManager;
    }
}
