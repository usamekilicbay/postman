using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private List<ItemCardConfig> cardConfigs;

    private int _cardCounter;

    #region Dependency Injection

    private DiContainer _diContainer;

    private InventoryManager _inventoryManager;
    private Card.Factory _cardFactory;

    [Inject]
    public void Construct(DiContainer diContainer,
        InventoryManager inventoryManager,
        Card.Factory cardFactory,
        Card card)
    {
        _inventoryManager = inventoryManager;
        _cardFactory = cardFactory;
        _diContainer = diContainer;
    }

    #endregion

    private void Awake()
    {
        SpawnCard();
    }

    public void SpawnCard()
    {
        if (_cardCounter == 10)
            _inventoryManager.CompleteRun();

        var card = _cardFactory.Create(_diContainer);
        var randomIndex = Random.Range(0, cardConfigs.Count);
        card.SetConfigs(cardConfigs[randomIndex]);

        //var card = Instantiate(cardPrefab, transform)
        //    .GetComponent<Card>();

        _cardCounter++;
    }
}
