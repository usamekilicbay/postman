using Merchant.Card;
using Merchant.Config.Item;
using Merchant.UI.Screen;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Merchant.Manager
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private List<ItemConfig> cardConfigs;

        private int _cardCounter;
        private CardBase _currentCard;

        #region Dependency Injection

        private DiContainer _diContainer;
        private GameManager _gameManager;
        private InventoryManager _inventoryManager;
        private ItemCard.Factory _itemCardFactory;
        private UIManagerBase _uiManager;
        private UIHomeScreen _uiHomeScreen;
        private UIGameScreen _uiGameScreen;

        [Inject]
        public void Construct(GameManager gameManager,
            DiContainer diContainer,
            ItemCard.Factory itemCardFactory)
        {
            _gameManager = gameManager;
            _diContainer = diContainer;
            _inventoryManager = _diContainer.Resolve<InventoryManager>();
            _itemCardFactory = itemCardFactory;
            _uiManager = _diContainer.Resolve<UIManagerBase>();
            _uiHomeScreen = _diContainer.Resolve<UIHomeScreen>();
            _uiGameScreen = _diContainer.Resolve<UIGameScreen>();
        }

        #endregion

        public void StartRun()
        {
            _cardCounter = 6;
            SpawnCard();
        }

        public void SpawnCard()
        {
            //if (_cardCounter == 0)
            if (_inventoryManager.IsTemporaryInventoryFull())
            {
                _gameManager.CompleteItemCollect();
                return;
            }

            _uiGameScreen.UpdateRemaningItemCountText(_cardCounter);
            var card = _itemCardFactory.Create();
            var randomIndex = Random.Range(0, cardConfigs.Count);
            card.SetCardConfigs(cardConfigs[randomIndex]);
            _currentCard = card;

            _cardCounter--;
        }

        public void CompleteRun()
        {
            if (!_currentCard.IsUsed)
                Destroy(_currentCard.gameObject);
        }
    }
}
