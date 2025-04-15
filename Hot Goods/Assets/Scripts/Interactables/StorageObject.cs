using UnityEngine;

public class StorageObject : MonoBehaviour, IInteractable
{
    //storageInventory;

    public void Interact()
    {
        Debug.Log("Opened wardrobe storage.");

    }

    private void Reset()
    {

    }
}
