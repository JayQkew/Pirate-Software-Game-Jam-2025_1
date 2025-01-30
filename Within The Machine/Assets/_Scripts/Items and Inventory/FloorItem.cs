using System.Collections;
using System.Collections.Generic;
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
}
