using UnityEngine;

public class WorkbenchObject : MonoBehaviour, IInteractable
{
    //storageInventory;
    public InputInventory inventoryBase;
    public InputInventory inventoryLayer;
    public OutputInventory inventoryOutput;

    private void Start()
    {
        inventoryBase.allowedTypes = new[] { ItemType.Base };
        inventoryLayer.allowedTypes = new[] { ItemType.Fix, ItemType.DIY };
        inventoryOutput.allowedTypes = new[] { ItemType.Base };
    }
    public void Interact()
    {
        Debug.Log("Opened workbench.");
        UIManager.Instance.OpenWorkbench(inventoryBase, inventoryLayer, inventoryOutput);
    }

    private void Reset()
    {
        inventoryBase = GetComponentsInChildren<InputInventory>()[0];
        inventoryLayer = GetComponentsInChildren<InputInventory>()[1];
        inventoryOutput = GetComponentInChildren<OutputInventory>();
    }

}
