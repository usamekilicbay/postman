using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Config", menuName = "Configs/Inventory Config")]
public class InventoryConfig : ScriptableObject
{
    [Tooltip("Maximum weight limit.")]
    public float CarryCapacity;
    [Tooltip("As percentage (%)")]
    public float ItemDiscardProbability;
}
