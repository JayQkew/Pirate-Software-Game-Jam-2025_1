using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;

    public void Drop(ItemSlot item)
    {
        GameObject obj = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<FloorItem>().itemSlot = item;
    }

    public void Drop(ItemSlot item, Vector3 position)
    {
        GameObject obj = Instantiate(itemPrefab, position, Quaternion.identity);
        obj.GetComponent<FloorItem>().itemSlot = item;
    }
}
