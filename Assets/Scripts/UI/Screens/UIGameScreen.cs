using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class UIGameScreen : UIScreenBase
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI remaningCardCountText;
    [Space(10)]
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform inventorySpawnParent;

    private List<InventorySlot> _inventorySlots = new();
    private List<InventoryItem> _inventoryItems = new();

    private InventoryItem.Factory _inventoryItemFactory;

    [Inject]
    public void Construct(InventoryItem.Factory inventoryItemFactory)
    {
        _inventoryItemFactory = inventoryItemFactory;
    }

    public void UpdateMoneyText(int money)
    {
        moneyText.SetText($"${money}");
    }

    public void UpdateRemaningItemCountText(int remaningCardCount)
    {
        remaningCardCountText.SetText($"Remaining Cards: {remaningCardCount}");
    }

    public void AddItemToInventory(ItemCardConfig item)
    {
        var inventorySlot = _inventorySlots.FirstOrDefault(x => !x.IsFull);

        if (inventorySlot == null)
        {
            Debug.Log("Inventory is full. New item can not be added!");

            return;
        }

        inventorySlot.FillSlot();
        var inventoryItem = _inventoryItemFactory.Create().GetComponent<InventoryItem>();
        inventoryItem.transform.SetParent(inventorySlot.transform, false);
        inventoryItem.SetItem(item);
        _inventoryItems.Add(inventoryItem);
    }

    public void DiscardItemFromInventory(InventoryItem inventoryItem)
    {
        var inventorySlot = inventoryItem.GetComponentInParent<InventorySlot>();
        inventorySlot.EmptySlot();
        _inventoryItems.Remove(inventoryItem);
        Destroy(inventoryItem);
    }

    public void CreateInventorySlots(int inventorySlotCount)
    {
        for (var i = 0; i < inventorySlotCount; i++)
        {
            var inventorySlot = Instantiate(inventorySlotPrefab, inventorySpawnParent)
                .GetComponent<InventorySlot>();
            _inventorySlots.Add(inventorySlot);
        }
    }
}
