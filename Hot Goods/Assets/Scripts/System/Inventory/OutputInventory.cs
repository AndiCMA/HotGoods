using UnityEngine;

public class OutputInventory : InventoryBase
{
    private void Reset()
    {
        slotCount = 1; // Default capacity for storage objects
    }

    private void Start()
    {
        if (slots == null || slots.Length != slotCount)
            slots = new InventoryItem[slotCount];
    }
}
