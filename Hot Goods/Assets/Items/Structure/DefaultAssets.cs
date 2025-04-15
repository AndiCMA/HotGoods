using UnityEngine;

[CreateAssetMenu(menuName = "Item/Default Assets")]
public class DefaultAssets : ScriptableObject
{
    public Sprite defaultBaseIcon;
    public Sprite defaultFixIcon;
    public Sprite defaultDIYIcon;
    public Sprite defaultConsumableIcon;
    public GameObject defaultCardboardBox;

    private static DefaultAssets _instance;
    public static DefaultAssets Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<DefaultAssets>("DefaultAssets");
            return _instance;
        }
    }

    public static Sprite GetIcon(ItemType type) => type switch
    {
        ItemType.Base => Instance.defaultBaseIcon,
        ItemType.Fix => Instance.defaultFixIcon,
        ItemType.DIY => Instance.defaultDIYIcon,
        ItemType.Consumable => Instance.defaultConsumableIcon,
        _ => null
    };

    public static GameObject GetModel() => Instance.defaultCardboardBox;
}
