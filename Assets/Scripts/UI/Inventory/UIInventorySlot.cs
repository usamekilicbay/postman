using UnityEngine;
using UnityEngine.EventSystems;

namespace Merchant.UI.Inventory
{
    public class UIInventorySlot : MonoBehaviour, IDropHandler
    {
        public bool IsEmpty { get; private set; }
        public UIInventoryItem CurrentItem { get; private set; }

        public void EmptySlot()
        {
            CurrentItem = null;
            IsEmpty = true;
        }

        public void AddItem(UIInventoryItem item, RectTransform itemRectTransform = default)
        {
            CurrentItem = item;
            IsEmpty = false;
            item.transform.SetParent(transform);
            itemRectTransform.anchoredPosition = Vector2.zero;
            itemRectTransform.localScale = Vector3.one;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent<UIInventoryItem>(out var draggedItem))
                draggedItem.OnEndDrag(eventData);
        }
    }
}
