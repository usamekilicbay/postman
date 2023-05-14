using UnityEngine;
using Zenject;

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
            .Bind<InventoryManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .Bind<DeckManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .Bind<CurrencyManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .Bind<CardFacade>()
            .AsSingle();

        Container
           .BindFactory<ItemCard, ItemCard.Factory>()
           .FromComponentInNewPrefab(itemCardPrefab);
        
        Container
           .BindFactory<AuctionCard, AuctionCard.Factory>()
           .FromComponentInNewPrefab(auctionCardPrefab);

        Container
            .BindFactory<InventoryItem, InventoryItem.Factory>()
            .FromComponentInNewPrefab(inventoryItemPrefab);

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
