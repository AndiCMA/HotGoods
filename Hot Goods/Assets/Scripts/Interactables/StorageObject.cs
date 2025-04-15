using UnityEngine;

public class StorageObject : MonoBehaviour, IInteractable
{
    //storageInventory;
    public StorageInventory inventory;

    public void Interact()
    {
        Debug.Log("Opened wardrobe storage.");
        UIManager.Instance.OpenStorage(inventory);
    }

    private void Reset()
    {
        inventory = GetComponentInChildren<StorageInventory>();
    }

}
