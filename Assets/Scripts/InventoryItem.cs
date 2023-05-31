using UnityEngine;
using Zenject;

public class InventoryItem
{
    public ItemCardConfig ItemConfig { get; private set; }
    public int StackCount { get; private set; }

    const int _stackLimit = 3;

    public void SetInventoryItem(ItemCardConfig itemConfig)
    {
        ItemConfig = itemConfig;
        StackCount = 1;
    }

    /// <summary>
    /// Adds specified amount items to the stack. 
    /// Amount parameter is 1 as default.
    /// If the amount is more than the stack can hold, 
    /// adds amount of to fill the remaining space and returns the remaning ones.
    /// </summary>
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> Remaining amount that can not be added to the stack </returns>
    public int AddItemToStack(int amount = 1)
    {
        var canBeAdded = amount - RemainingSpace();
        StackCount += canBeAdded;

        return amount - canBeAdded;
    }

    // TODO: Fix this method
    /// <summary>
    /// Removes from the stack inventory if amount parameteres is not passed,
    /// adds 1 to the stack.
    /// If the amount is more than the stack limit, adds as much as possible and returns the remaning ones.
    /// </summary>
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> Remaining amount that can not be added to the stack </returns>
    public int RemoveItemFromStack(int amount = 1)
    {
        StackCount--;

        return 0;
    }

    public bool IsStackEmpty()
        => StackCount == 0;

    public bool IsStackFull()
        => StackCount == _stackLimit;

    public float GetTotalWeight()
        => ItemConfig.Weight * StackCount;

    private int RemainingSpace()
    {
        return _stackLimit - StackCount;
    }

    public class Factory : PlaceholderFactory<InventoryItem>
    {

    }
}
