using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image icon;
    public Text quantityText;

    private PlayerInventory inventory;
    private int slotIndex;

    public void Setup(PlayerInventory inv, int index)
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

    public bool HasItem()
    {
        var item = inventory.slots[slotIndex];
        return item != null && item.item != null;
    }

    public Sprite GetIcon() => icon.sprite;
    public PlayerInventory GetInventory() => inventory;
    public int GetIndex() => slotIndex;

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragController.Instance.StartDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragController.Instance.Drag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var target = eventData.pointerEnter?.GetComponent<InventorySlotUI>();
        DragController.Instance.EndDrag(target);
    }
}
