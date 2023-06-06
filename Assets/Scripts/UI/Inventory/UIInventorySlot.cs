using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Merchant.UI.Inventory.Slot
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public abstract class UIInventorySlot : MonoBehaviour, IDropHandler
    {
        public UIInventoryItem CurrentItem { get; private set; }

        public bool IsEmpty 
            => CurrentItem == null;

        public void AddItem(UIInventoryItem item)
        {
            CurrentItem = item;
            CurrentItem.UpdateSlot(this);
        }

        public void RemoveItem()
        {
            CurrentItem = null;
        }

        public void OnDrop(PointerEventData eventData)
        {
            //if (eventData.pointerDrag.TryGetComponent<UIInventoryItem>(out var draggedItem))
            //    draggedItem.OnEndDrag(eventData);
        }

        public class Factory : PlaceholderFactory<UIInventorySlot>
        {
            ///
        }
    }
}
