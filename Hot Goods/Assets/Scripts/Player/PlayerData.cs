using System.Collections.Generic;
using UnityEngine;
public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int money;
    public int health;
    public Vector3 lastPosition;
    public List<InventoryItem> savedInventory = new();
    public PlayerInventory runtimeInventory;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        runtimeInventory = FindObjectOfType<PlayerInventory>();
        if (runtimeInventory != null)
        {
            runtimeInventory.InitializeFrom(savedInventory);
            UIManager.Instance.playerInventoryPanel.RefreshAll();
        }
    }

    void Start()
    {
        if (savedInventory.Count == 0)
        {
            AddDebugItem<BaseItemData>(0);
            AddDebugItem<FixItemData>(5);
            AddDebugItem<DIYItemData>(12);
            AddDebugItem<ConsumableItemData>(15);

            runtimeInventory.InitializeFrom(savedInventory);
        }
    }
    void AddDebugItem<T>(int slotIndex) where T : ItemData
    {
    #if UNITY_EDITOR
        string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        if (guids.Length == 0) return;

        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
        var item = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);

        if (item != null)
        {
            if(item.itemType != ItemType.Base)
                savedInventory.Add(new InventoryItem(item, 20, slotIndex));
            else
                savedInventory.Add(new InventoryItem(item, 1, slotIndex));
            Debug.Log($"[DEBUG] Injected {item.name} into slot {slotIndex}");
        }
    #endif
    }

}
