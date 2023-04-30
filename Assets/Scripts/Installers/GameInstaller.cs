using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject cardPrefab;

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
            .BindFactory<DiContainer, Card, Card.Factory>()
            .FromComponentInNewPrefab(cardPrefab);
    }

    public void InstallCard(DiContainer subContainer)
    {
        subContainer
           .BindFactory<DiContainer, Card, Card.Factory>()
           .FromComponentInNewPrefab(cardPrefab);
        subContainer
            .Bind<InventoryManager>()
            .FromComponentInHierarchy()
            .AsSingle();
        subContainer
             .Bind<CurrencyManager>()
            .FromComponentInHierarchy()
            .AsSingle();
        subContainer
            .Bind<InventoryManager>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}
