using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] itemsInInventory;
    
    public int inventorySize;

    public void DisplayInventory()
    {
        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            ItemSlot item = itemsInInventory[i];
            if (item.isEmpty())
                Debug.Log($"{i}: empty");
            else
                Debug.Log($"{i}: {item.itemData.name} - {item.stackValue}");
        }
    }

    public void AddItemToInventory(Item item)
    {
        //check if can stack
        
        // check if not fill
        
        // make new item instance

        int i = 0;

        int freeSlot = -1;

        while (i < inventorySize)
        {
            ItemSlot checkedItem = itemsInInventory[i];
            if (!checkedItem.isEmpty())
            {
                if (checkedItem.itemData.name == item.name)
                    if (checkedItem.stackValue < checkedItem.itemData.stackAmount)
                    {
                        checkedItem.AddToStack();
                        return;
                    }
            }
            else
            {
                if (freeSlot < 0)
                    freeSlot = i;
            }

            i++;
        }

        if (freeSlot != -1)
        {
            itemsInInventory[freeSlot] = new ItemSlot(item);
            return;
        }
        
        
        Debug.Log("Inventory is full. Cannot add item.");
    }

    public void AddItemToInventory(ItemSlot item)
    {
        int i = 0;

        int freeSlot = -1;

        while (i < inventorySize)
        {
            ItemSlot checkedItem = itemsInInventory[i];
            if (!checkedItem.isEmpty())
            {
                if (checkedItem.itemData.name == item.itemData.name)
                    if (checkedItem.stackValue + item.stackValue < checkedItem.itemData.stackAmount)
                    {
                        checkedItem.AddToStack(item.stackValue);
                        return;
                    }
            }
            else
            {
                if (freeSlot < 0)
                    freeSlot = i;
            }

            i++;
        }

        if (freeSlot != -1)
        {
            itemsInInventory[freeSlot] = item;
            return;
        }
        
        
        Debug.Log("Inventory is full. Cannot add item.");
    }
    
    
}
