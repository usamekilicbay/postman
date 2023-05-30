public class InventoryItem
{
    public ItemCardConfig ItemConfig { get; private set; }
    public int StackCount { get; private set; }

    public void SetInventoryItem(ItemCardConfig itemConfig)
    {
        ItemConfig = itemConfig;
    }

    public float GetTotalWeight() 
        => ItemConfig.Weight * StackCount;
}
