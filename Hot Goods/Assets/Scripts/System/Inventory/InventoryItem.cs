[System.Serializable]
public class InventoryItem
{
    public ItemData item;
    public int quantity;
    public int slotIndex;

    public InventoryItem(ItemData item, int quantity, int slotIndex)
    {
        this.item = item;
        this.quantity = quantity;
        this.slotIndex = slotIndex;
    }
}
