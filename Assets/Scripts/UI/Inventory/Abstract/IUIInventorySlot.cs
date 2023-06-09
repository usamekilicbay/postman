using UnityEngine;

namespace Merchant.UI.Inventory.Abstract.Slot
{
    public interface IUIInventorySlot
    {
        void AddItem(UIInventoryItem item);

        void RemoveItem();
    }
}