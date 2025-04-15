using UnityEngine;

[CreateAssetMenu(menuName = "Item/Fix Item")]
public class FixItemData : ItemData
{
    public NegativeData fixesNegative;

    private void OnEnable()
    {
        itemType = ItemType.Fix;
        stackable = true;
    }

    public bool CanFix(BaseItemData item)
    {
        return item.negatives.Contains(fixesNegative);
    }
}
