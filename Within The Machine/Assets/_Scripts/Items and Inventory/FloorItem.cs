using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FloorItem : MonoBehaviour
{
    public where whereTag;
    
    public ParalaxLayers paralaxLayers;
    public LayerMask targetLayer;
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
        paralaxLayers = GameObject.FindGameObjectWithTag("Paralax").GetComponent<ParalaxLayers>();
    }

    public void SetItem(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        GetComponent<SpriteRenderer>().sprite = itemSlot.itemData.icon;
    }
    
    public void SetItem(ItemSlot itemSlot, ParalaxLayers paralaxLayers)
    {
        this.itemSlot = itemSlot;
        GetComponent<SpriteRenderer>().sprite = itemSlot.itemData.icon;
        this.paralaxLayers = paralaxLayers;
    }
    
    

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, targetLayer);

        if (hit.collider != null)
        {
            // Move left
            transform.position += Vector3.left * (paralaxLayers.speedRatios[5] * paralaxLayers.machineSpeed * 175 * Time.deltaTime);
        }
    }
}
