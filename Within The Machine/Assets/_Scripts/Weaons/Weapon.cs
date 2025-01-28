using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Inventory inputInventory;
    
    [Header("Stats")]
    public float damage;
    public float range;
    public float fireRate;
    public float shotAmmoCost;
    
    
    [Header("Checks")]
    public bool canFire;
    public float currentAmmo;
    public bool canSeeEnemy;

    

    public void Update()
    {
        if (!canSeeEnemy) return;
        
        if (!canFire || !CheckAmmo()) return;

        if (FireWeapon())
        {
            currentAmmo -= shotAmmoCost;
            StartCoroutine(StartCoolDown());
        }
    }
    
    public virtual bool FireWeapon()
    {
        Debug.Log($"Fire {gameObject.name}!");
        return true;
    }


    public bool CheckAmmo()
    {
        if (currentAmmo >= shotAmmoCost) return true;
        
        if (LoadAmmo())
        {
            Debug.Log($"Reloading {gameObject.name}");
            return true;
        }        
        else
        {
            Debug.Log($"Not enough ammo");
            return false;
        }

        //return LoadAmmo();
    }

    public bool LoadAmmo()
    {
        bool loadedAmmo = false;

        int step = 0;
        
        while (!loadedAmmo && step < inputInventory.itemsInInventory.Length)
        {
            ItemSlot itemSlot = inputInventory.GetItemSlot(step);
            //Debug.Log(itemSlot.isEmpty());
            if (!itemSlot.isEmpty())
            {
                loadedAmmo = true;
                currentAmmo += itemSlot.itemData.ammoAmount;
                inputInventory.RemoveItems(itemSlot.itemData);
            }

            step++;
        }
        return loadedAmmo;
    }


    protected IEnumerator StartCoolDown()
    {
        Debug.Log("StartCoolDown");
        canFire = false;
        yield return new WaitForSeconds(1 / fireRate);
        canFire = true;
    }
}
