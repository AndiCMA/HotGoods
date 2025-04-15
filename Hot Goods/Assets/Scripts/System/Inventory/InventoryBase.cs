using UnityEngine;

public abstract class InventoryBase : MonoBehaviour
{
    public int slotCount = 30;
    public InventoryItem[] slots;
    public ItemType[] allowedTypes = new[] {ItemType.Base, ItemType.Fix, ItemType.DIY, ItemType.Consumable};

    protected virtual void Awake()
    {
        slots = new InventoryItem[slotCount];
    }

    public virtual bool IsItemAccepted(ItemData item)
    {
        foreach (var type in allowedTypes)
        {
            if (item.itemType == type)
                return true;
        }
        return false;
    }
    public virtual void InitializeFrom(System.Collections.Generic.List<InventoryItem> sourceItems)
    {
        slots = new InventoryItem[slotCount];
        foreach (var item in sourceItems)
        {
            if (item.slotIndex >= 0 && item.slotIndex < slotCount)
            {
                slots[item.slotIndex] = new InventoryItem(item.item, item.quantity, item.slotIndex);
            }
        }
    }

    public virtual bool MoveItem(int from, int to)
    {
        if (from == to || from < 0 || to < 0 || from >= slots.Length || to >= slots.Length)
            return false;

        var itemA = slots[from];
        var itemB = slots[to];

        if (itemA == null) return false;

        if (itemB == null)
        {
            slots[to] = itemA;
            slots[from] = null;
            itemA.slotIndex = to;
            return true;
        }

        if (itemA.item == itemB.item && itemA.item.stackable)
        {
            itemB.quantity += itemA.quantity;
            slots[from] = null;
            return true;
        }

        (slots[from], slots[to]) = (slots[to], slots[from]);
        (slots[from].slotIndex, slots[to].slotIndex) = (from, to);
        return true;
    }
    public bool CopyItem(ItemData item, int to, int quantity)
    {
        Debug.Log("Copy item");
        Debug.Log($"Allowed items: {string.Join(", ", allowedTypes)}");
        if(!IsItemAccepted(item)){
            Debug.Log("Item not accepted");
            return false;
        }
        if (slots[to] == null)
        {
            slots[to] = new InventoryItem(item, quantity, to);
            return true;
        }
        else
        {
            if (slots[to].item != null &&
                slots[to].item.id == item.id &&
                slots[to].item.name == item.name &&
                item.stackable &&
                item.itemType != ItemType.Base)
            {
                slots[to].quantity += quantity;
                return true;
            }
        }

        return false;
    }

    public virtual bool SplitStack(int from, int to, int amount)
    {
        if (from == to || amount <= 0) return false;
        if (from < 0 || to < 0 || from >= slots.Length || to >= slots.Length) return false;

        var fromItem = slots[from];
        if (fromItem == null || slots[to] != null || amount >= fromItem.quantity) return false;

        fromItem.quantity -= amount;
        slots[to] = new InventoryItem(fromItem.item, amount, to);
        return true;
    }

    public virtual bool TryAddItem(ItemData item, int quantity)
    {
        Debug.Log("TryAddItem");
        if(!IsItemAccepted(item)){
            Debug.Log("Item not accepted");
            Debug.Log($" imported {item.itemType } not allowed here");
            return false;
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].item == item && item.stackable)
            {
                slots[i].quantity += quantity;
                return true;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new InventoryItem(item, quantity, i);
                return true;
            }
        }

        return false;
    }
    public virtual bool RemoveItemAt(int slotIndex, int amount)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return false;
        var item = slots[slotIndex];
        if (item == null || amount <= 0 || amount > item.quantity) return false;

        item.quantity -= amount;
        if (item.quantity <= 0)
            slots[slotIndex] = null;

        return true;
    }

}
