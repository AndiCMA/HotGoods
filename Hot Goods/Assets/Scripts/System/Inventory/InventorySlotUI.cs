using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler
{
    public Image icon;
    public Text quantityText;

    private InventoryBase inventory;
    private int slotIndex;

    public void Setup(InventoryBase inv, int index)
    {
        inventory = inv;
        slotIndex = index;
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        var item = inventory.slots[slotIndex];
        if (item != null && item.item != null)
        {
            icon.sprite = item.item.icon != null ? item.item.icon : DefaultAssets.GetIcon(item.item.itemType);
            icon.enabled = true;
            quantityText.text = item.item.stackable ? item.quantity.ToString() : "";
        }
        else
        {
            icon.enabled = false;
            quantityText.text = "";
        }
    }

    public InventoryItem GetCurrentItem() => inventory.slots[slotIndex];
    public InventoryBase GetInventory() => inventory;
    public int GetIndex() => slotIndex;
    public Sprite GetIcon() => icon.sprite;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var item = GetCurrentItem();
        if (item == null || item.quantity <= 0) return;

        DragController.Instance.StartDrag(this, item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragController.Instance.UpdateDragPosition(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var pointer = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        foreach (var r in results)
        {
            var targetSlot = r.gameObject.GetComponent<InventorySlotUI>();
            if (targetSlot != null)
            {
                DragController.Instance.CompleteDrag(targetSlot);
                return;
            }
        }

        DragController.Instance.CancelDrag();
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (!DragController.Instance.IsDragging()) return;

        var currentItem = GetCurrentItem();
        if (currentItem == null || currentItem.quantity <= 1 || !currentItem.item.stackable) return;

        DragController.Instance.AdjustSplitAmount(eventData.scrollDelta.y);
    }
}
