using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public int slotCount = 30;
    public InventoryItem[] slots;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeFrom(List<InventoryItem> sourceItems)
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

    public bool MoveItem(int from, int to)
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
        if(slots[to].item == null)
            slots[to] = new InventoryItem(item, quantity, to);
        else{
            if(slots[to].item.id == item.id && item.itemType != ItemType.Base){
                slots[to].quantity += quantity;
            }else{
                return false;
            }
        }
        return true;
    }

    public bool SplitStack(int from, int to, int amount)
    {
        if (from == to || amount <= 0) return false;
        if (from < 0 || to < 0 || from >= slots.Length || to >= slots.Length) return false;

        var fromItem = slots[from];
        if (fromItem == null || slots[to] != null || amount >= fromItem.quantity) return false;

        fromItem.quantity -= amount;
        slots[to] = new InventoryItem(fromItem.item, amount, to);
        return true;
    }

    public bool TryAddItem(ItemData item, int quantity)
    {
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
} 