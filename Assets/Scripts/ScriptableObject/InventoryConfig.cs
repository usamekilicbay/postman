using UnityEngine;

namespace Merchant.Config
{
    [CreateAssetMenu(fileName = "New Inventory Config", menuName = "Configs/Inventory Config")]
    public class InventoryConfig : ScriptableObject
    {
        [Tooltip("Maximum weight limit.")]
        public float CarryCapacity;
        [Tooltip("As percentage (%)")]
        public float ItemDiscardProbability;
        [Space(10)]
        [Header("Inventory Sizes")]
        public int MainInventorySlotCount;
        public int AuctionInventorySlotCount;
        public int TemporaryInventorySlotCount;
    }
}
