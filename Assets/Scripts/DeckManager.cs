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

    private UIManagerBase _uiManager;
    private UIHomeScreen _uiHomeScreen;
    private UIGameScreen _uiGameScreen;

    [Inject]
    public void Construct(DiContainer diContainer,
        Card.Factory cardFactory)
    {
        _diContainer = diContainer;
        _inventoryManager = _diContainer.Resolve<InventoryManager>();
        _cardFactory = cardFactory;
        _uiManager = _diContainer.Resolve<UIManagerBase>();
        _uiHomeScreen = _diContainer.Resolve<UIHomeScreen>();
        _uiGameScreen = _diContainer.Resolve<UIGameScreen>();
    }

    #endregion

    private void Awake()
    {
        _cardCounter = 6;
        SpawnCard();
    }

    public void SpawnCard()
    {
        if (_cardCounter == 0)
        {
            CompleteRun();
            return;
        }

        _uiGameScreen.UpdateRemaningCardCountText(_cardCounter);
        var card = _cardFactory.Create();
        var randomIndex = Random.Range(0, cardConfigs.Count);
        card.SetConfigs(cardConfigs[randomIndex]);

        _cardCounter--;
    }

    private void CompleteRun()
    {
        _inventoryManager.CompleteRun();
        _uiManager.ShowScreen(_uiHomeScreen);
    }
}
