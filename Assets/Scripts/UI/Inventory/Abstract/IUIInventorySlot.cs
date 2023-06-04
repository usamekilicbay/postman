using UnityEngine;

namespace Merchant.UI.Inventory.Abstract
{
    public interface IUIInventorySlot
    {
        void AddItem(UIInventoryItem item, RectTransform itemRectTransform = default);

        void RemoveItem();
    }
}