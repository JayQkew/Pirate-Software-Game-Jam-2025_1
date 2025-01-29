using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;

    public bool PickupItem(Item item)
    {
        return inventory.AddItemToInventory(item);
    }

    public void DropItem(int index)
    {
        // drop item
        // make physical item
        
        inventory.RemoveItemIndex(index);
    }


    public void PutInCraftingStation(CraftingStation craftingStation)
    {
        //
    }
    
}
