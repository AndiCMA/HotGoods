using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Base Item")]
public class BaseItemData : ItemData
{
    public List<PerkData> perks = new();
    public List<NegativeData> negatives = new();
    public TrendsData trend;

    private void OnEnable()
    {
        itemType = ItemType.Base;
        stackable = false;
    }
}
