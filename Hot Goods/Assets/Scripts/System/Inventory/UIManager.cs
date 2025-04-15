using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject playerInventoryPanelObject;
    public InventoryPanelUI playerInventoryPanel;
    public GameObject storageInventoryPanelObject;
    public InventoryPanelUI storageInventoryPanel;
    public GameObject workbenchInventoryPanelObject;
    public InventoryPanelUI workbenchInventoryPanel;
    private PlayerBehavior playerMoves;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerMoves = FindObjectOfType<PlayerBehavior>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            TogglePlayerInventory();
            
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (storageInventoryPanelObject.activeSelf)
            {
                CloseStorage();
            }
            else if (playerInventoryPanelObject.activeSelf)
            {
                TogglePlayerInventory();
            }
        }
    }

    public void TogglePlayerInventory()
    {
        bool isActive = playerInventoryPanelObject.activeSelf;
        playerInventoryPanelObject.SetActive(!isActive);

        if (!isActive)
        {
            playerInventoryPanel.Init(PlayerInventory.Instance);
        }

        UpdateUIState();
    }

    public void OpenStorage(StorageInventory storage)
    {
        storageInventoryPanelObject.SetActive(true);
        storageInventoryPanel.Init(storage);
        TogglePlayerInventory(); // optional, if you want both open
        UpdateUIState();
    }

    public void CloseStorage()
    {
        storageInventoryPanelObject.SetActive(false);
        UpdateUIState();
    }

    public void OpenWorkbench(InputInventory inventoryBase , InputInventory inventoryLayer, OutputInventory inventoryOutput)
    {
        workbenchInventoryPanelObject.SetActive(true);
        var panels = workbenchInventoryPanelObject.GetComponentsInChildren<InventoryPanelUI>(true);
        if (panels.Length >= 3)
        {
            panels[0].Init(inventoryBase);
            panels[1].Init(inventoryLayer);
            panels[2].Init(inventoryOutput);
        }
        else
        {
            Debug.LogError("Workbench panels missing or not properly assigned.");
        }

        TogglePlayerInventory(); // optional, if you want both open
        UpdateUIState();
    }

    public void CloseWorkbench()
    {
        workbenchInventoryPanelObject.SetActive(false);
        UpdateUIState();
    }
    private void UpdateUIState()
    {
        bool anyOpen = playerInventoryPanelObject.activeSelf || storageInventoryPanelObject.activeSelf || workbenchInventoryPanelObject.activeSelf;

        Cursor.lockState = anyOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = anyOpen;

        if (playerMoves != null)
            playerMoves.allowMovement = !anyOpen;
    }
}
