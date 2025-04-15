using UnityEngine;

public class StorageInventory : InventoryBase
{
    private void Reset()
    {
        slotCount = 20; // Default capacity for storage objects
    }

    private void Start()
    {
        if (slots == null || slots.Length != slotCount)
            slots = new InventoryItem[slotCount];
    }
}
