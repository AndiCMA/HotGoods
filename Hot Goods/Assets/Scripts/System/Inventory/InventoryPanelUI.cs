using UnityEngine;

public class InventoryPanelUI : MonoBehaviour
{
    public InventorySlotUI slotPrefab;
    private InventoryBase inventory;

    public void Init(InventoryBase inv)
    {
        inventory = inv;
        if (inventory == null || inventory.slots == null) return;

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            var slot = Instantiate(slotPrefab, transform);
            slot.Setup(inventory, i);
        }
    }

    public void RefreshAll()
    {
        foreach (var slot in GetComponentsInChildren<InventorySlotUI>())
            slot.UpdateSlot();
    }
}
