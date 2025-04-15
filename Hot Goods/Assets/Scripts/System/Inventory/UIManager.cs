using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject playerInventoryPanelObject;
    public InventoryPanelUI playerInventoryPanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            TogglePlayerInventory();
    }

    public void TogglePlayerInventory()
    {
        bool isActive = playerInventoryPanelObject.activeSelf;
        playerInventoryPanelObject.SetActive(!isActive);

        if (!isActive)
        {
            playerInventoryPanel.Init();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
