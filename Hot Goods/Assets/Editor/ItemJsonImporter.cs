#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class ItemDataDTO
{
    public string itemName;
    public string description;
    public int estimatedValue;
    public string size;
    public string state;
    public string iconName;
    public string prefabName;
}

[System.Serializable]
public class ItemDataList
{
    public ItemDataDTO[] items;
}

public class ItemJsonImporter : MonoBehaviour
{
    [MenuItem("Tools/Import Items From JSON")]
    public static void ImportItems()
    {
        // string path = "Assets/Resources/items.json";
        // if (!File.Exists(path)) { Debug.LogError("Missing JSON file!"); return; }

        // string json = File.ReadAllText(path);
        // ItemDataList dataList = JsonUtility.FromJson<ItemDataList>(json);

        // foreach (var dto in dataList.items)
        // {
        //     ItemData item = ScriptableObject.CreateInstance<ItemData>();

        //     item.itemName = dto.itemName;
        //     item.description = dto.description;
        //     item.estimatedValue = dto.estimatedValue;

        //     // Enums
        //     item.size = System.Enum.Parse<ItemSize>(dto.size);
        //     item.state = System.Enum.Parse<ItemState>(dto.state);

        //     // Load asset
        //     var sprite = Resources.Load<Sprite>("items/icons/default");
        //     Debug.Log(sprite != null ? "✅ Sprite loaded" : "❌ Sprite NOT found");

        //     item.icon = sprite;
        //     item.prefab = Resources.Load<GameObject>(dto.prefabName);

        //     // Save asset
        //     string assetPath = $"Assets/Items/{dto.itemName}.asset";
        //     AssetDatabase.CreateAsset(item, assetPath);
        // }

        // AssetDatabase.SaveAssets();
        // AssetDatabase.Refresh();
        // Debug.Log("✅ Item import complete.");
    }
}
#endif
