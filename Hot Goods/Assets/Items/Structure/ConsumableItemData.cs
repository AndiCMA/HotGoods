using System.Collections.Generic;
using UnityEngine;

public enum EffectType { Hunger, Thirst, Speed, Energy }

[System.Serializable]
public class ConsumableEffect
{
    public EffectType effect;
    public float value;
    public float duration;
}

[CreateAssetMenu(menuName = "Item/Consumable Item")]
public class ConsumableItemData : ItemData
{
    public List<ConsumableEffect> effects = new();

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
        stackable = true;
    }
}
