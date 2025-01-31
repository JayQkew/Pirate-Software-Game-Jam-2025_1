using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "Scriptable Object/New Crafting Recipe", order = 2)]
public class CraftingRecipe : ScriptableObject
{
    [Serializable]
    public struct RecipeItem
    {
        public Item item;
        public int amount;
    }
    
    public List<RecipeItem> inputItems = new List<RecipeItem>();
    //public List<RecipeItem> outputItems = new List<RecipeItem>();
    public List<RecipeItem> outputItem;
}
