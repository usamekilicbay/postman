using Merchant.UI.Screen;
using UnityEngine;
using Zenject;

namespace Merchant.Manager
{
    public class GameManager : MonoBehaviour
    {
        private UIManagerBase _uiManager;
        private InventoryManager _inventoryManager;
        private ICurrencyManager _currencyManager;
        private DeckManager _deckManager;
        private AuctionDeckManager _auctionDeckManager;
        private UIHomeScreen _uiHomeScreen;
        private UIGameScreen _uiGameScreen;
        private UIGameResultScreen _uiGameResultScreen;
        private UIAuctionPreparationScreen _uiAuctionPreparationScreen;

        [Inject]
        public void Construct(UIManagerBase uiManager,
            InventoryManager inventoryManager,
            ICurrencyManager currencyManager,
            DeckManager deckManager,
            AuctionDeckManager auctionDeckManager,
            UIHomeScreen uiHomeScreen,
            UIGameScreen uiGameScreen,
            UIGameResultScreen uiGameResultScreen,
            UIAuctionPreparationScreen uiAuctionPreparationScreen)
        {
            _uiManager = uiManager;
            _inventoryManager = inventoryManager;
            _currencyManager = currencyManager;
            _deckManager = deckManager;
            _auctionDeckManager = auctionDeckManager;
            _uiHomeScreen = uiHomeScreen;
            _uiGameScreen = uiGameScreen;
            _uiGameResultScreen = uiGameResultScreen;
            _uiAuctionPreparationScreen = uiAuctionPreparationScreen;
        }

        //TODO: Delete on deploy
        private void Start()
        {
            StartItemCollectRun();
            _uiManager.ShowScreen(_uiGameScreen);
        }

        public void StartItemCollectRun()
        {
            _deckManager.StartRun();
            _inventoryManager.StartRun();
        }

        public void CompleteItemCollect()
        {
            _deckManager.CompleteRun();
            _uiManager.ShowScreen(_uiGameResultScreen);
        }

        public void StartAuction()
        {
            _auctionDeckManager.StartRun();
        }

        public void CompleteAuction()
        {
            _auctionDeckManager.CompleteRun();
            _inventoryManager.CompleteAuctionRun();
        }
    }
}
