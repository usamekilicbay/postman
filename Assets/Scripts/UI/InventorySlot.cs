using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public bool IsFull { get; private set; }

    public void FillSlot()
    {
        IsFull = true;
    }

    public void EmptySlot()
    {
        IsFull = false;
    }
}
