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
        Debug.Log("Complete drag");
        if (!isDragging || originSlot == null || targetSlot == null || fullItem == null)
        {
            CancelDrag();
            return;
        }
        Debug.Log("element found");

        var originInventory = originSlot.GetInventory();
        var targetInventory = targetSlot.GetInventory();
        var originIndex = originSlot.GetIndex();

        bool isSplit = splitAmount < fullItem.quantity || Input.GetKey(KeyCode.LeftShift);
        bool isSplitHalf = Input.GetKey(KeyCode.LeftShift);
        

        Debug.Log($"Is split {isSplit}");

        if (isSplit)
        {
            if(isSplitHalf){
                splitAmount=fullItem.quantity/2;
            }
            bool success = targetInventory.CopyItem(fullItem.item, targetSlot.GetIndex(), splitAmount);
            if (success)
            {
                originInventory.slots[originIndex].quantity -= splitAmount;
                if (originInventory.slots[originIndex].quantity <= 0)
                    originInventory.slots[originIndex] = null;
            }
        }
        else
        {
            if(originInventory == targetInventory){
                originInventory.MoveItem(originIndex, targetSlot.GetIndex());
            }else{
                bool success = targetInventory.CopyItem(fullItem.item, targetSlot.GetIndex(), fullItem.quantity);
                if (success)
                {
                    originInventory.slots[originIndex] = null;
                }
            }
        }

        originSlot.UpdateSlot();
        targetSlot.UpdateSlot();

        EndDrag();
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
