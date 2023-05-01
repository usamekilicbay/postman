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

    private UIGameScreen _uiGameScreen;

    [Inject]
    public void Construct(DiContainer diContainer,
        Card.Factory cardFactory)
    {
        _diContainer = diContainer;
        _inventoryManager = _diContainer.Resolve<InventoryManager>();
        _cardFactory = cardFactory;
        _uiGameScreen = _diContainer.Resolve<UIGameScreen>();
    }

    #endregion

    private void Awake()
    {
        _cardCounter = 3;
        SpawnCard();
    }

    public void SpawnCard()
    {
        if (_cardCounter == 0)
            _inventoryManager.CompleteRun();

        _uiGameScreen.UpdateRemaningCardCountText(_cardCounter);
        var card = _cardFactory.Create();
        var randomIndex = Random.Range(0, cardConfigs.Count);
        card.SetConfigs(cardConfigs[randomIndex]);

        _cardCounter--;
    }
}
