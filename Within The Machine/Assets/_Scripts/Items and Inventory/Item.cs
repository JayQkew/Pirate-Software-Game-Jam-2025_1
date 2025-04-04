using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/New Item", order = 1)]
public class Item : ScriptableObject
{
    public string name;
    public Sprite icon;
    public string description;
    public int stackAmount;

    [Header("Fuel")]
    public bool isFuel;
    public float fuelAmount;
    
    [Header("Ammo")]
    public bool isAmmo;
    public float ammoAmount;
    
    [Header("Repair")]
    public bool isRepair;
    public float repairAmount;
}


[System.Serializable]
public class ItemSlot
{
    public Item itemData;
    public int stackValue;


    public ItemSlot(Item item, int stackValue = 1)
    {
        itemData = item;
        this.stackValue = stackValue;
    }

    public void AddToStack(int amount = 1)
    {
        if (stackValue + amount > itemData.stackAmount)
        {
            Debug.LogWarning("Stack cannot be larger than item stack amount");
            return;
        }
        
        stackValue += amount;
    }

    public void RemoveFromStack(int amount = 1)
    {
        if (stackValue - amount < 0)
        {
            Debug.LogWarning("Stack cannot be less than 0");
            return;
        }
        
        stackValue -= amount;
        if (stackValue == 0)
            MakeEmpty();
    }

    public bool isEmpty()
    {
        return  itemData == null || stackValue == 0;
    }

    public void MakeEmpty()
    {
        itemData = null;
        stackValue = 0;
    }

    public void SetItem(ItemSlot slot)
    {
        itemData = slot.itemData;
        stackValue = slot.stackValue;
    }
}
