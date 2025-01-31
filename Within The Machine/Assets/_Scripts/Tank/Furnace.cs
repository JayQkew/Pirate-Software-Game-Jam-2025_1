using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    public bool isPowering;

    [Header("Burn Rates")]
    [SerializeField]
    float defaultBurnRate;
    [SerializeField]
    float tankSpeedMultily;
    public SpeedManager _speedManager;
    //public float tankSpeedTest;

    [Header("Inventory and fuel")]
    public Inventory inputInventory;
    public float fuelLevel;


    [Header("Current Fuel")]
    public Item currentFuel;
    public float fuelUsage;
    
    [SerializeField] public Animator animator;


    private void Update()
    {
        if (currentFuel == null)
        {
            GetNextFuel();
        }    
        else
            BurnFuel();
    }

    void GetNextFuel()
    {
        
        currentFuel = null;

        int step = 0;

        while (currentFuel == null && step < inputInventory.itemsInInventory.Length)
        {
            ItemSlot itemSlot = inputInventory.GetItemSlot(step);
            //Debug.Log(itemSlot.isEmpty());
            if (!itemSlot.isEmpty())
            {
                currentFuel = itemSlot.itemData;
                inputInventory.RemoveItems(currentFuel);
            }

            step++;
        }

        isPowering = (currentFuel != null);
    }


    void BurnFuel()
    {
        float burnRate = defaultBurnRate + _speedManager.currentSpeed * tankSpeedMultily;


        fuelUsage += burnRate * Time.deltaTime;
        if (fuelUsage >= currentFuel.fuelAmount)
        {
            currentFuel = null;
            Debug.Log("Fuel done");
            fuelUsage = 0;
        }
    }
    
    public bool PlaceItemInCraftingStation(ItemSlot itemSlot)
    {
        return inputInventory.AddItemToInventory(itemSlot);
    }

    
}
