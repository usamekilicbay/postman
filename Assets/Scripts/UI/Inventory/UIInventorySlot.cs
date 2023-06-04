using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Merchant.UI.Inventory.Slot
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public abstract class UIInventorySlot : MonoBehaviour, IDropHandler
    {
        public bool IsEmpty { get; private set; }
        public UIInventoryItem CurrentItem { get; private set; }

        public void AddItem(UIInventoryItem item, RectTransform itemRectTransform = default)
        {
            CurrentItem = item;
            IsEmpty = false;
            item.transform.SetParent(transform);
            itemRectTransform.anchoredPosition = Vector2.zero;
            itemRectTransform.localScale = Vector3.one;
        }

        public void RemoveItem()
        {
            CurrentItem = null;
            IsEmpty = true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent<UIInventoryItem>(out var draggedItem))
                draggedItem.OnEndDrag(eventData);
        }

        public class Factory : PlaceholderFactory<UIInventorySlot>
        {
            ///
        }
    }
}
