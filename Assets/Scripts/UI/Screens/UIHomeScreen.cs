using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIHomeScreen : UIScreenBase
{
    [SerializeField] private Button startRunButton;
    [SerializeField] private Button auctionButton;

    private UIGameScreen _uiGameScreen;
    private UIAuctionPreparationScreen _uiAuctionPreparationScreen;

    [Inject]
    public void Construct(UIGameScreen gameScreen,
        UIAuctionPreparationScreen auctionPreparationScreen)
    {
        _uiGameScreen = gameScreen;
        _uiAuctionPreparationScreen = auctionPreparationScreen;
    }

    private void Awake()
    {
        startRunButton.onClick
            .AddListener(() => uiManager.ShowScreen(_uiGameScreen));

        auctionButton.onClick
            .AddListener(() => uiManager.ShowScreen(_uiAuctionPreparationScreen));
    }
}
