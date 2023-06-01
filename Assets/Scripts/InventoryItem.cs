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
    public int IncreaseStack(int count = 1)
    {
        var remainingCount = 0;

        if (StackCount + count <= _stackLimit)
            StackCount += count;
        else
        {
            var availableSpace = _stackLimit - StackCount;
            StackCount = _stackLimit;
            remainingCount = count - availableSpace;
        }

        Debug.LogWarning($"Added {count} items to the stack. Remaining count: {remainingCount}");

        return remainingCount;
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
    public int ReduceStack(int count = 1)
    {
        var remainingCount = 0;

        if (count >= StackCount)
        {
            remainingCount = count - StackCount;
            StackCount = 0;
        }
        else
            StackCount -= count;

        Debug.LogWarning($"Removed {count} items from the stack. Remaining count: {StackCount}");

        return remainingCount;
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
