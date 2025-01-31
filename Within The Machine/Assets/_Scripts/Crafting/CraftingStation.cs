using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingStation : MonoBehaviour
{
    public CraftingRecipe[] recipes;
    [SerializeField] private int currentRecipe;
    
    [Header("Inventory")]
    [SerializeField]
    Inventory inputInventory;
    [SerializeField]
    Inventory outputInventory;

    [Header("References")] 
    [SerializeField]
    private DropItem _dropItem;

    private void Update()
    {
        if (CheckRecipeIngredients())
            ProduceItem();
    }

    bool CheckRecipeIngredients()
    {
        CraftingRecipe recipe = recipes[currentRecipe];
        
        bool canCreate = true;

        int step = 0;

        while (canCreate && step < recipe.inputItems.Count)
        {
            canCreate = inputInventory.CountItem(recipe.inputItems[step].item) >= recipe.inputItems[step].amount;
            step++;
        }
        return canCreate;
    }

    public void ProduceItem()
    {
        if (!CheckRecipeIngredients())
        {
            Debug.Log("Needs more items to craft!.");
            return;
        }

        foreach (var inputItem in recipes[currentRecipe].inputItems)
        {
            inputInventory.RemoveItems(inputItem.item, inputItem.amount);
        }

        List<CraftingRecipe.RecipeItem> outputItems = recipes[currentRecipe].outputItem;

        for (int i = 0; i < outputItems.Count; i++)
        {
            //outputInventory.AddItemToInventory(outputItem.item);
            _dropItem.Drop(outputItems[i].item);
        }

        //Debug.Log("Crafted!");
    }

    public bool PlaceItemInCraftingStation(ItemSlot itemSlot)
    {
        return inputInventory.AddItemToInventory(itemSlot);
    }
}
