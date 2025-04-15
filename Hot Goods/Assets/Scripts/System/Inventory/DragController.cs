using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragController : MonoBehaviour
{
    public static DragController Instance;

    public Image dragImage;

    private InventorySlotUI originSlot;
    private InventoryItem fullItem;
    private int splitAmount;
    private bool isDragging = false;

    void Awake()
    {
        Instance = this;
        dragImage.gameObject.SetActive(false);
    }

    public void StartDrag(InventorySlotUI slot, InventoryItem originalItem)
    {
        originSlot = slot;
        fullItem = originalItem;
        splitAmount = originalItem.quantity;

        dragImage.sprite = slot.GetIcon();
        dragImage.rectTransform.position = Input.mousePosition;
        dragImage.gameObject.SetActive(true);

        isDragging = true;
    }

    public void UpdateDragPosition(Vector2 screenPos)
    {
        if (isDragging)
            dragImage.rectTransform.position = screenPos;
    }

    public void AdjustSplitAmount(float scrollDelta)
    {
        if (!isDragging || fullItem == null || !fullItem.item.stackable) return;

        int max = fullItem.quantity;
        splitAmount += (int)Mathf.Sign(scrollDelta);
        splitAmount = Mathf.Clamp(splitAmount, 1, max);

        Debug.Log($"[Drag] Adjusted split amount: {splitAmount}/{fullItem.quantity}");
    }

    public void CompleteDrag(InventorySlotUI targetSlot)
    {
        if (!ValidateDrop(targetSlot)) return;

        var originInventory = originSlot.GetInventory();
        var targetInventory = targetSlot.GetInventory();
        var originIndex = originSlot.GetIndex();
        var targetIndex = targetSlot.GetIndex();

        if (!targetInventory.IsItemAccepted(fullItem.item))
        {
            Debug.LogWarning($"[Drag] Item '{fullItem.item.name}' not accepted in '{targetInventory.name}'");
            EndDrag();
            return;
        }

        if (ShouldSplitDrag())
            HandleSplitDrag(originInventory, targetInventory, originIndex, targetIndex);
        else
            HandleFullDrag(originInventory, targetInventory, originIndex, targetIndex);

        originSlot.UpdateSlot();
        targetSlot.UpdateSlot();
        EndDrag();
    }

    private bool ValidateDrop(InventorySlotUI targetSlot)
    {
        if (!isDragging || originSlot == null || targetSlot == null || fullItem == null)
        {
            CancelDrag();
            return false;
        }
        return true;
    }

    private bool ShouldSplitDrag()
    {
        return splitAmount < fullItem.quantity || Input.GetKey(KeyCode.LeftShift);
    }

    private void HandleSplitDrag(InventoryBase origin, InventoryBase target, int from, int to)
    {
        if (Input.GetKey(KeyCode.LeftShift))
            splitAmount = fullItem.quantity / 2;

        bool success = target.CopyItem(fullItem.item, to, splitAmount);
        if (success)
            origin.RemoveItemAt(from, splitAmount);
    }

    private void HandleFullDrag(InventoryBase origin, InventoryBase target, int from, int to)
    {
        if (origin == target)
        {
            origin.MoveItem(from, to);
        }
        else
        {
            bool success = target.CopyItem(fullItem.item, to, fullItem.quantity);
            if (success)
                origin.RemoveItemAt(from, fullItem.quantity);
        }
    }

    public void CancelDrag()
    {
        if (isDragging) EndDrag();
    }

    private void EndDrag()
    {
        dragImage.gameObject.SetActive(false);
        isDragging = false;
        fullItem = null;
        originSlot = null;
        splitAmount = 0;
    }

    public bool IsDragging() => isDragging;
}
