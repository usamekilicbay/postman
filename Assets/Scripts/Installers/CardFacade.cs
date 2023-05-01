using Zenject;

public class CardFacade
{
    public DeckManager DeckManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }
    public CurrencyManager CurrencyManager { get; private set; }

    [Inject]
    public void Construct(DeckManager deckManager,
        InventoryManager inventoryManager,
        CurrencyManager currencyManager)
    {
        DeckManager = deckManager;
        InventoryManager = inventoryManager;
        CurrencyManager = currencyManager;
    }
}
