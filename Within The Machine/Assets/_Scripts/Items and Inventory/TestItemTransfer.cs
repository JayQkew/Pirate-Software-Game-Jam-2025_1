using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemTransfer : MonoBehaviour
{
    public Inventory inventory1;
    public Inventory inventory2;

    public void MoveTo1(int index)
    {
        if (inventory1.AddItemToInventory(inventory2.itemsInInventory[index]))
        {
            inventory2.itemsInInventory[index].MakeEmpty();
        }
    }

    public void MoveTo2(int index)
    {
        if (inventory2.AddItemToInventory(inventory1.itemsInInventory[index]))
        {
            inventory1.itemsInInventory[index].MakeEmpty();
        }
    }
}
