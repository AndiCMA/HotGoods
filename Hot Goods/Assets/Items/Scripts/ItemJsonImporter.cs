using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class ItemJsonImporter : EditorWindow
{
    [MenuItem("Tools/Import Game Items From JSON")]
    public static void Import()
    {
        string basePath = "Assets/Items/JSON";
        string targetPath = "Assets/Items/Generated";

        if (!Directory.Exists(basePath))
        {
            Debug.LogError("JSON folder not found: " + basePath);
            return;
        }

        ImportPerks(Path.Combine(basePath, "Perks.json"), Path.Combine(targetPath, "Perks"));
        ImportNegatives(Path.Combine(basePath, "Negatives.json"), Path.Combine(targetPath, "Negatives"));
        ImportTrends(Path.Combine(basePath, "Trends.json"), Path.Combine(targetPath, "Trends"));
        ImportBaseItems(Path.Combine(basePath, "Items_Base.json"), Path.Combine(targetPath, "BaseItems"));
        ImportFixItems(Path.Combine(basePath, "Items_Fix.json"), Path.Combine(targetPath, "FixItems"));
        ImportDIYItems(Path.Combine(basePath, "Items_DIY.json"), Path.Combine(targetPath, "DIYItems"));
        ImportConsumables(Path.Combine(basePath, "Items_Consumables.json"), Path.Combine(targetPath, "Consumables"));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("✅ Item import complete");
    }

    static void ImportPerks(string jsonPath, string savePath)
    {
        EnsureFolder(savePath);
        var list = JsonHelper.FromJson<PerkDataJson>(File.ReadAllText(jsonPath));
        foreach (var data in list)
        {
            var so = ScriptableObject.CreateInstance<PerkData>();
            string safeName = ObjectNames.GetUniqueName(null, data.perkName.Replace(" ", "_"));
            so.name = safeName;
            so.perkName = data.perkName;
            so.bonusValue = data.bonusValue;
            AssetDatabase.CreateAsset(so, Path.Combine(savePath, $"{safeName}.asset"));
        }
    }

    static void ImportNegatives(string jsonPath, string savePath)
    {
        EnsureFolder(savePath);
        var list = JsonHelper.FromJson<NegativeDataJson>(File.ReadAllText(jsonPath));
        foreach (var data in list)
        {
            var so = ScriptableObject.CreateInstance<NegativeData>();
            string safeName = ObjectNames.GetUniqueName(null, data.negativeName.Replace(" ", "_"));
            so.name = safeName;
            so.negativeName = data.negativeName;
            so.penaltyValue = data.penaltyValue;
            AssetDatabase.CreateAsset(so, Path.Combine(savePath, $"{safeName}.asset"));
        }
    }

    static void ImportTrends(string jsonPath, string savePath)
    {
        EnsureFolder(savePath);
        var list = JsonHelper.FromJson<TrendsDataJson>(File.ReadAllText(jsonPath));
        foreach (var data in list)
        {
            var so = ScriptableObject.CreateInstance<TrendsData>();
            string safeName = ObjectNames.GetUniqueName(null, data.trendName.Replace(" ", "_"));
            so.name = safeName;
            so.trendName = data.trendName;
            AssetDatabase.CreateAsset(so, Path.Combine(savePath, $"{safeName}.asset"));
        }
    }

    static void ImportBaseItems(string jsonPath, string savePath)
    {
        EnsureFolder(savePath);
        var list = JsonHelper.FromJson<BaseItemDataJson>(File.ReadAllText(jsonPath));
        foreach (var data in list)
        {
            var so = ScriptableObject.CreateInstance<BaseItemData>();
            string safeName = ObjectNames.GetUniqueName(null, data.itemName.Replace(" ", "_"));
            so.name = safeName;
            so.itemName = data.itemName;
            so.description = data.description;
            so.baseValue = data.baseValue;
            so.packageSize = Enum.Parse<PackageSize>(data.packageSize);
            so.trend = LoadAssetById<TrendsData>(data.trendID);
            so.perks = data.perkIDs.ConvertAll(id => LoadAssetById<PerkData>(id));
            so.negatives = data.negativeIDs.ConvertAll(id => LoadAssetById<NegativeData>(id));
            AssetDatabase.CreateAsset(so, Path.Combine(savePath, $"{safeName}.asset"));
        }
    }

    static void ImportFixItems(string jsonPath, string savePath)
    {
        EnsureFolder(savePath);
        var list = JsonHelper.FromJson<FixItemDataJson>(File.ReadAllText(jsonPath));
        foreach (var data in list)
        {
            var so = ScriptableObject.CreateInstance<FixItemData>();
            string safeName = ObjectNames.GetUniqueName(null, data.itemName.Replace(" ", "_"));
            so.name = safeName;
            so.itemName = data.itemName;
            so.description = data.description;
            so.baseValue = data.baseValue;
            so.packageSize = Enum.Parse<PackageSize>(data.packageSize);
            so.fixesNegative = LoadAssetById<NegativeData>(data.fixesNegativeID);
            AssetDatabase.CreateAsset(so, Path.Combine(savePath, $"{safeName}.asset"));
        }
    }

    static void ImportDIYItems(string jsonPath, string savePath)
    {
        EnsureFolder(savePath);
        var list = JsonHelper.FromJson<DIYItemDataJson>(File.ReadAllText(jsonPath));
        foreach (var data in list)
        {
            var so = ScriptableObject.CreateInstance<DIYItemData>();
            string safeName = ObjectNames.GetUniqueName(null, data.itemName.Replace(" ", "_"));
            so.name = safeName;
            so.itemName = data.itemName;
            so.description = data.description;
            so.baseValue = data.baseValue;
            so.packageSize = Enum.Parse<PackageSize>(data.packageSize);
            so.addsPerk = LoadAssetById<PerkData>(data.addsPerkID);
            so.bonusRange = new Vector2(data.bonusRange[0], data.bonusRange[1]);
            AssetDatabase.CreateAsset(so, Path.Combine(savePath, $"{safeName}.asset"));
        }
    }

    static void ImportConsumables(string jsonPath, string savePath)
    {
        EnsureFolder(savePath);
        var list = JsonHelper.FromJson<ConsumableItemDataJson>(File.ReadAllText(jsonPath));
        foreach (var data in list)
        {
            var so = ScriptableObject.CreateInstance<ConsumableItemData>();
            string safeName = ObjectNames.GetUniqueName(null, data.itemName.Replace(" ", "_"));
            so.name = safeName;
            so.itemName = data.itemName;
            so.description = data.description;
            so.baseValue = data.baseValue;
            so.packageSize = Enum.Parse<PackageSize>(data.packageSize);
            so.effects = data.effects;
            AssetDatabase.CreateAsset(so, Path.Combine(savePath, $"{safeName}.asset"));
        }
    }

    static void EnsureFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }

    static T LoadAssetById<T>(int id) where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null && path.Contains($"_{id}") == false && asset.name.EndsWith(id.ToString()))
                return asset;
        }
        Debug.LogWarning($"❌ Could not find {typeof(T).Name} with ID {id}");
        return null;
    }

    // --- Helper JSON structs ---
    [System.Serializable] public class PerkDataJson { public int id; public string perkName; public float bonusValue; }
    [System.Serializable] public class NegativeDataJson { public int id; public string negativeName; public float penaltyValue; }
    [System.Serializable] public class TrendsDataJson { public int id; public string trendName; }
    [System.Serializable] public class BaseItemDataJson
    {
        public int id;
        public string itemName;
        public string description;
        public float baseValue;
        public string packageSize;
        public List<int> perkIDs;
        public List<int> negativeIDs;
        public int trendID;
    }
    [System.Serializable] public class FixItemDataJson
    {
        public int id;
        public string itemName;
        public string description;
        public float baseValue;
        public string packageSize;
        public int fixesNegativeID;
    }
    [System.Serializable] public class DIYItemDataJson
    {
        public int id;
        public string itemName;
        public string description;
        public float baseValue;
        public string packageSize;
        public int addsPerkID;
        public List<float> bonusRange;
    }
    [System.Serializable] public class ConsumableItemDataJson
    {
        public int id;
        public string itemName;
        public string description;
        public float baseValue;
        public string packageSize;
        public List<ConsumableEffect> effects;
    }

    public static class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            return JsonUtility.FromJson<Wrapper<T>>(WrapJson(json)).items;
        }
        private static string WrapJson(string raw) => "{\"items\":" + raw + "}";
        [System.Serializable] private class Wrapper<T> { public List<T> items; }
    }
}