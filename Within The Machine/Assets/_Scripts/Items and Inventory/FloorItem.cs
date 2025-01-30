using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FloorItem : MonoBehaviour
{
    public where whereTag;
    public enum where
    {
        floor,
        hand,
    }
    public ItemSlot itemSlot;

    private void Awake()
    {
        if (itemSlot.isEmpty()) return;

        GetComponent<SpriteRenderer>().sprite = itemSlot.itemData.icon;
    }

    public void SetItem(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        GetComponent<SpriteRenderer>().sprite = itemSlot.itemData.icon;
    }
}
