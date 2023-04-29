using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] InventoryConfig inventoryConfig;

    private float _burden;

    private List<ItemCardConfig> temporaryItems;
    private List<ItemCardConfig> items;

    void Start()
    {
        temporaryItems = new List<ItemCardConfig>();
        items = new List<ItemCardConfig>();
    }

    public void CollectItem(ItemCardConfig item)
    {
        if (_burden >= inventoryConfig.CarryCapacity)
        {
            if (Random.Range(0f, 1f) > inventoryConfig.ItemDiscardProbability)
                return;

            var selectedItem = ScriptableObject.CreateInstance<ItemCardConfig>();

            do
            {
                var randomIndex = Random.Range(0, temporaryItems.Count);
                selectedItem = temporaryItems[randomIndex];

            } while (_burden - selectedItem.Weight + item.Weight <= inventoryConfig.CarryCapacity);

            temporaryItems.Remove(selectedItem);
        }

        temporaryItems.Add(item);
        _burden += item.Weight;
    }

    public void CompleteRun()
    {
        foreach (var item in temporaryItems)
            items.Add(item);
    }
}
