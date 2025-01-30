using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;

    public void Drop(ItemSlot item)
    {
        GameObject obj = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        ItemSlot newItem = new ItemSlot(item.itemData,item.stackValue);
        obj.GetComponent<FloorItem>().SetItem(newItem);
    }

    public void Drop(ItemSlot item, Vector3 position)
    {
        GameObject obj = Instantiate(itemPrefab, position, Quaternion.identity);
        ItemSlot newItem = new ItemSlot(item.itemData,item.stackValue);
        obj.GetComponent<FloorItem>().SetItem(newItem);
    }
}
