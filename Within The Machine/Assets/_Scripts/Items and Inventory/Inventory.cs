using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] itemsInInventory;
    
    
    
    [Header("Constraints")]
    public int inventorySize;

    [SerializeField] 
    private bool isRestricted;

    [SerializeField] 
    private List<Item> onlyContains;
    
    [SerializeField]
    private List<Item> notContains;

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

    private void Awake()
    {
        inventorySize = itemsInInventory.Length;
    }

    public bool AddItemToInventory(Item item)
    {
        if (ISItemRestricted(item)) return false;

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
                        return true;
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
            return true;
        }
        
        
        Debug.Log("Inventory is full. Cannot add item.");
        return false;
    }

    public bool AddItemToInventory(ItemSlot item)
    {
        if (ISItemRestricted(item.itemData)) return false;
        
        int i = 0;

        int freeSlot = -1;

        while (i < inventorySize)
        {
            ItemSlot checkedItem = itemsInInventory[i];
            if (!checkedItem.isEmpty())
            {
                if (checkedItem.itemData.name == item.itemData.name)
                {
                    if (checkedItem.stackValue + item.stackValue <= checkedItem.itemData.stackAmount)
                    {
                        checkedItem.AddToStack(item.stackValue);
                        return true;
                    }
                    else
                    {
                        int diff = checkedItem.itemData.stackAmount - checkedItem.stackValue;
                        checkedItem.AddToStack(diff);
                        item.RemoveFromStack(diff);
                    }
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
            itemsInInventory[freeSlot].SetItem(item);
            return true;
        }
        
        
        Debug.Log("Inventory is full. Cannot add item.");
        return false;
    }

    bool ISItemRestricted(Item item)
    {
        if (!isRestricted) return false;
        
        if (onlyContains.Count >0)
        {
            if (!onlyContains.Contains(item))
            {
                Debug.Log($"{item.name} cannot be placed here.");
                return true;
            }
        }
        else if (notContains.Count >0)
        {
            if (notContains.Contains(item))
            {
                Debug.Log($"{item.name} cannot be placed here.");
                return true;
            }
        }
        
        return false;
    }

    public int CountItem(Item item)
    {
        int count = 0;

        foreach (ItemSlot itemSlot in itemsInInventory)
        {
            if (itemSlot.itemData == item)
                count += itemSlot.stackValue;
        }
        
        return count;
    }

    public bool RemoveItems(Item item, int amount = 1)
    {
        if (amount > CountItem(item))
        {
            Debug.Log($"Insufficient amount of {item.name} to remove.");
            return false;
        }

        int index = 0;
        while (amount > 0)
        {
            ItemSlot itemSlot = itemsInInventory[index];
            if (!itemSlot.isEmpty())
            {
                if (itemSlot.itemData.name == item.name)
                {
                    if (itemSlot.stackValue < amount)
                    {
                        amount -= itemSlot.stackValue;
                        itemSlot.RemoveFromStack(itemSlot.stackValue);
                    }
                    else
                    {
                        itemSlot.RemoveFromStack(amount);
                        amount = 0;
                    }
                }
            }
            index++;
        }
        return true;
    }

    public ItemSlot GetItemSlot(int index)
    {
        return itemsInInventory[index];
    }


    public int NumberOfItemsInInventory()
    {
        int count = 0;

        foreach (ItemSlot itemSlot in itemsInInventory)
        {
            count += itemSlot.stackValue;
        }

        return count;
    }

    public void RemoveItemIndex(int index)
    {
        itemsInInventory[index].MakeEmpty();
    }

    public int CountItems()
    {
        int count = 0;
        foreach (var item in itemsInInventory)
        {
            if (!item.isEmpty())
                count++;
        }
        return count;
    }
}
