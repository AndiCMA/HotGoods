using UnityEngine;

public enum ItemType { Base, Fix, DIY, Consumable }
public enum PackageSize { S, M, L, XL, XXL }

public abstract class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public float baseValue;
    public Sprite icon;
    public GameObject model;
    public ItemType itemType;
    public bool stackable = true;
    public PackageSize packageSize = PackageSize.M;

    public virtual Sprite GetIcon() => icon != null ? icon : DefaultAssets.GetIcon(itemType);
    public virtual GameObject GetModel() => model != null ? model : DefaultAssets.GetModel();
    public float GetScale() => ItemHelper.GetScaleFromPackage(packageSize);
}
