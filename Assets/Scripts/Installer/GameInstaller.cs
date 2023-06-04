using Merchant.Card;
using Merchant.Inventory;
using Merchant.Manager;
using Merchant.UI.Dialog;
using Merchant.UI.Inventory;
using Merchant.UI.Inventory.Slot;
using Merchant.UI.Screen;
using UnityEngine;
using Zenject;

namespace Merchant.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject itemCardPrefab;
        [SerializeField] private GameObject auctionCardPrefab;
        [SerializeField] private GameObject inventoryItemPrefab;

        public override void InstallBindings()
        {

            //Container
            //   .Bind<TouchManager>()
            //   .FromComponentInHierarchy()
            //   .AsSingle();

            Container
                .Bind<GameManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<InventoryManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<DeckManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<AuctionDeckManager>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container
                .Bind<ICurrencyManager>()
                .To<CurrencyManager>()
                .AsSingle();

            Container
                .Bind<CardFacade>()
                .AsSingle();

            Container
                .BindFactory<InventoryItem, InventoryItem.Factory>();

            Container
               .BindFactory<ItemCard, ItemCard.Factory>()
               .FromComponentInNewPrefab(itemCardPrefab);

            Container
               .BindFactory<AuctionCard, AuctionCard.Factory>()
               .FromComponentInNewPrefab(auctionCardPrefab);

            Container
                .BindFactory<UIInventoryItem, UIInventoryItem.Factory>()
                .FromComponentInNewPrefab(inventoryItemPrefab);

            //Container
            //    .BindFactory<UIInventorySlot, UIInventorySlot.Factory>()
            //    .To<>

            #region UI

            Container
                .Bind<UIManagerBase>()
                .FromComponentInHierarchy()
                .AsSingle();

            var screens = FindObjectsOfType<UIScreenBase>(true);
            foreach (var screen in screens)
                Container.Bind(screen.GetType()).FromComponentsInHierarchy().AsSingle();

            var dialogs = FindObjectsOfType<UIDialogBase>(true);
            foreach (var dialog in dialogs)
                Container.Bind(dialog.GetType()).FromComponentsInHierarchy().AsSingle();

            #endregion
        }
    }
}
