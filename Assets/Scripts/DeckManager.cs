using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private List<ItemCardConfig> cardConfigs;

    private int _cardCounter;

    private InventoryManager inventoryManager;

    private void Awake()
    {
        SpawnCard();
    }

    public void SpawnCard()
    {
        if (_cardCounter == 10)
            inventoryManager.CompleteRun();

        var card = Instantiate(cardPrefab, transform)
            .GetComponent<Card>();
        //card.gameObject.SetActive(false);
        var randomIndex = Random.Range(0, cardConfigs.Count);
        card.SetConfigs(cardConfigs[randomIndex]);

        _cardCounter++;
    }
}
