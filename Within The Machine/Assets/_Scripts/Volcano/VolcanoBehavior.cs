using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VolcanoBehavior : MonoBehaviour
{
    [Header("Volcano Setup")]
    [SerializeField] private Transform VolcanoCentre;
    [SerializeField] private float LeftOffset = -1f;
    [SerializeField] private float RightOffset = 1f;

    [Header("Volcano Eruption")]
    [SerializeField] private float UpwardForce = 30f;
    [SerializeField] private float Gravity = 10f;
    [SerializeField] private float shootLeft = -15f;
    [SerializeField] private float shootRight = 15f;
    [SerializeField] private int SortingLayer = 3;
    
    public void VolcanoProcess(GameObject item)
    {
        PutInValcano(item);
        SpewOutValcano(item);
        Collider2D collider2D = item.GetComponent<Collider2D>();
        collider2D.enabled = true;
    }

    private void PutInValcano(GameObject item)
    {
        Collider2D collider2D = item.GetComponent<Collider2D>();
        collider2D.enabled = false;
        SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
        sr.sortingOrder = -10;
        float OffsetX = Random.Range(LeftOffset, RightOffset);
        item.transform.position = VolcanoCentre.position + new Vector3(OffsetX, 0, 0);
    }

    private void SpewOutValcano(GameObject item)
    {
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        rb.gravityScale = Gravity;
        float ShootVar = Random.Range(shootLeft, shootRight);
        rb.AddForce(new Vector2(ShootVar, UpwardForce), ForceMode2D.Impulse);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        SpriteRenderer sr = other.gameObject.GetComponent<SpriteRenderer>();
        sr.sortingOrder = SortingLayer;
    }
}
