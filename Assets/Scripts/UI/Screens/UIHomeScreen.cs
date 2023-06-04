using Merchant.Manager;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Merchant.UI.Screen
{
    public class UIHomeScreen : UIScreenBase
    {
        [SerializeField] private Button startRunButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button auctionButton;

        private GameManager _gameManager;
        private UIGameScreen _uiGameScreen;
        private UIAuctionPreparationScreen _uiAuctionPreparationScreen;
        private UIInventoryScreen _uiInventoryScreen;

        [Inject]
        public void Construct(GameManager gameManager,
            UIGameScreen gameScreen,
            UIAuctionPreparationScreen auctionPreparationScreen,
            UIInventoryScreen inventoryScreen)
        {
            _gameManager = gameManager;
            _uiGameScreen = gameScreen;
            _uiInventoryScreen = inventoryScreen;
            _uiAuctionPreparationScreen = auctionPreparationScreen;
        }

        private void Awake()
        {
            startRunButton.onClick
                .AddListener(StartItemCollectRun);

            inventoryButton.onClick
                .AddListener(ShowInventoryScreen);

            auctionButton.onClick
                .AddListener(ShowAuctionPreparationScreen);
        }

        private void StartItemCollectRun()
        {
            _gameManager.StartItemCollectRun();
            uiManager.ShowScreen(_uiGameScreen);
        }

        private void ShowInventoryScreen()
        {
            uiManager.ShowScreen(_uiInventoryScreen);
        }

        private void ShowAuctionPreparationScreen()
        {
            uiManager.ShowScreen(_uiAuctionPreparationScreen);
        }
    }
}
