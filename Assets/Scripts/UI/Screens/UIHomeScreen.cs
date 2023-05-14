using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIHomeScreen : UIScreenBase
{
    [SerializeField] private Button startRunButton;
    [SerializeField] private Button auctionButton;

    private GameManager _gameManager;
    private UIGameScreen _uiGameScreen;
    private UIAuctionPreparationScreen _uiAuctionPreparationScreen;

    [Inject]
    public void Construct(GameManager gameManager,
        UIGameScreen gameScreen,
        UIAuctionPreparationScreen auctionPreparationScreen)
    {
        _gameManager = gameManager;
        _uiGameScreen = gameScreen;
        _uiAuctionPreparationScreen = auctionPreparationScreen;
    }

    private void Awake()
    {
        startRunButton.onClick
            .AddListener(StartItemCollectRun);

        auctionButton.onClick
            .AddListener(StartAuctionRun);
    }

    private void StartItemCollectRun()
    {
        _gameManager.StartItemCollectRun();
        uiManager.ShowScreen(_uiGameScreen);
    }

    private void StartAuctionRun()
    {
        _gameManager.StartAuction();
        uiManager.ShowScreen(_uiAuctionPreparationScreen);
    }
}
