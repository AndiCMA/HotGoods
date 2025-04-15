using UnityEngine;

[CreateAssetMenu(menuName = "Item/DIY Item")]
public class DIYItemData : ItemData
{
    public PerkData addsPerk;
    public Vector2 bonusRange;

    private void OnEnable()
    {
        itemType = ItemType.DIY;
        stackable = true;
    }

    public bool CanApplyTo(BaseItemData item)
    {
        return !item.perks.Contains(addsPerk);
    }
}
