using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;

    public GameObject leftItem;
    public GameObject rightItem;
    
    public DropItem _dropItem;

    public bool PickupItem(ItemSlot item)
    {
        if (inventory.AddItemToInventory(item))
        {
            UpdateHandItems();
            return true;
        }

        return false;
    }

    public void DropItem(int index)
    {
        // drop item
        // make physical item

        if (index == 0)
        {
            _dropItem.Drop(inventory.itemsInInventory[index],leftItem.transform.position);
        }
        else
        {
            _dropItem.Drop(inventory.itemsInInventory[index],rightItem.transform.position);
        }
        
        inventory.RemoveItemIndex(index);
        UpdateHandItems();
    }


    public void PutInCraftingStation(CraftingStation craftingStation)
    {
        //

        UpdateHandItems();
    }

    void UpdateHandItems()
    {
        ItemSlot item = inventory.itemsInInventory[0];
        if (item.isEmpty())
        {
            leftItem.SetActive(false);
        }
        else
        {
            leftItem.SetActive(true);
            leftItem.GetComponent<SpriteRenderer>().sprite = item.itemData.icon;
        }

        item = inventory.itemsInInventory[1];
        if (item.isEmpty())
        {
            rightItem.SetActive(false);
        }
        else
        {
            rightItem.SetActive(true);
            rightItem.GetComponent<SpriteRenderer>().sprite = item.itemData.icon;
        }

    }
    
}
