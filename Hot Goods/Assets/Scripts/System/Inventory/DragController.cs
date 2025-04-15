using UnityEngine;
using UnityEngine.UI;

public class DragController : MonoBehaviour
{
    public static DragController Instance;

    public Image dragImage;
    private InventorySlotUI originSlot;

    void Awake()
    {
        Instance = this;
        dragImage.gameObject.SetActive(false);
    }

    public void StartDrag(InventorySlotUI slot)
    {
        if (!slot.HasItem()) return;

        originSlot = slot;
        dragImage.sprite = slot.GetIcon();
        dragImage.rectTransform.position = Input.mousePosition;
        dragImage.gameObject.SetActive(true);
    }

    public void Drag()
    {
        dragImage.rectTransform.position = Input.mousePosition;
    }

    public void EndDrag(InventorySlotUI targetSlot)
    {
        dragImage.gameObject.SetActive(false);

        if (targetSlot == null || originSlot == null) return;
        originSlot.GetInventory().MoveItem(originSlot.GetIndex(), targetSlot.GetIndex());
        targetSlot.UpdateSlot();
        originSlot.UpdateSlot();
    }

    public void CancelDrag()
    {
        dragImage.gameObject.SetActive(false);
        originSlot = null;
    }

    public InventorySlotUI GetOriginSlot() => originSlot;
}
