using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public bool IsEmpty { get; private set; } = true;

    public void FillSlot()
    {
        IsEmpty = false;
    }
}
