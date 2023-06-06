using Merchant.Card;
using Merchant.Config;
using Merchant.Config.Item;
using Merchant.UI.Screen;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Merchant.Manager
{
    public class AuctionDeckManager : MonoBehaviour
    {
        //[SerializeField] Sprite tempCustomerArtwork;
        //private int _offerCounter;
        //private int _lastOffer;
        //private ItemCardConfig _itemOnSale;

        //private readonly string[] _customerNames = { "Jayda", "Charlie", "Paige", "Sean", "Kerri", "Milena", "Tomas", "Malorie", "Drake", "Caelan", "Macie", "Gilbert", "Amiah", "Davonte", "Jean", "Luke", "Amanda", "Silvia", "Essence", "Ada", "Devyn", "Lyndsay", "Maleah", "Brady", "Angelina", "Daron", "Amari", "Fatima", "Kayla", "India" };

        //#region Dependency Injection

        //private GameManager _gameManager;
        //private DiContainer _diContainer;
        //private InventoryManager _inventoryManager;
        //private AuctionCard.Factory _auctionCardFactory;
        //private UIManagerBase _uiManager;
        //private UIHomeScreen _uiHomeScreen;
        //private UIGameScreen _uiGameScreen;
        //[Inject]
        //public void Construct(GameManager gameManager,
        //    DiContainer diContainer,
        //    AuctionCard.Factory auctionCardFactory)
        //{
        //    _gameManager = gameManager;
        //    _diContainer = diContainer;
        //    _inventoryManager = _diContainer.Resolve<InventoryManager>();
        //    _auctionCardFactory = auctionCardFactory;
        //    _uiManager = _diContainer.Resolve<UIManagerBase>();
        //    _uiHomeScreen = _diContainer.Resolve<UIHomeScreen>();
        //    _uiGameScreen = _diContainer.Resolve<UIGameScreen>();
        //}

        //#endregion

        //public void StartRun()
        //{
        //    StartNewItemAuction();
        //}

        //public void StartNewItemAuction()
        //{
        //    if (!_inventoryManager.AuctionItems.Any())
        //    {
        //        _gameManager.CompleteAuction();
        //        return;
        //    }

        //    _itemOnSale = _inventoryManager.AuctionItems.First().ItemConfig;

        //    _offerCounter = 6;
        //    _lastOffer = 0;
        //    SpawnCard();
        //}

        //public void SpawnCard()
        //{
        //    _uiGameScreen.UpdateRemaningItemCountText(_offerCounter);

        //    //if (_offerCounter == 0)
        //    //    _lastOffer = _itemOnSale.Price;

        //    var randomCustomerNameIndex = Random.Range(0, _customerNames.Length);
        //    var customerName = _customerNames[randomCustomerNameIndex];
        //    var randomOffer = Random.Range(1, Mathf.RoundToInt(_itemOnSale.Price * 0.1f));
        //    var offer = _lastOffer + randomOffer;

        //    var card = _auctionCardFactory.Create();
        //    card.SetCardConfigs(_itemOnSale, customerName, offer, tempCustomerArtwork);

        //    _offerCounter--;
        //}

        //public void CompleteRun()
        //{
        //    Destroy(_itemOnSale);
        //}
    }
}
