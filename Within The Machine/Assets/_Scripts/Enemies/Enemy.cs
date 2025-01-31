using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("Stats")] public float maxHealth;
    public float moveSpeed;
    public float damage;


    [Header("Current Stats")] public float health;
    public float debuffSpeed;

    [Header("Drops")] public Item[] dropItems;
    public float[] dropChances;

    [Header("References")]
    //public SpeedManager _speedManager;
    public ParalaxLayers _paralaxLayers;
    public Rigidbody2D rb;
    public DropItem _dropItem;

    public float tankMoveOffset;
    private Color originalColor;

    private void Awake()
    {
        health = maxHealth;
        //currentSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        originalColor = Color.white;
        _paralaxLayers = GameObject.FindGameObjectWithTag("Paralax").GetComponent<ParalaxLayers>();
    }

    private void Update()
    {
        float xVelocity = moveSpeed;
        xVelocity -= debuffSpeed;
        xVelocity += _paralaxLayers.speedRatios[5] * _paralaxLayers.machineSpeed * tankMoveOffset;
        //rb.velocity = new Vector2(-xVelocity, 0);
        transform.Translate(-xVelocity * Time.deltaTime, 0, 0);
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageEffect());
        }
    }

    private IEnumerator DamageEffect()
    {
        foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = Color.red;
        }
        debuffSpeed = moveSpeed * .5f;
        yield return new WaitForSeconds(.1f);
        debuffSpeed = 0;
        foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = Color.white;
        }
    }

    void Die()
    {
        float totalChance = 0f;

        foreach (float chance in dropChances)
        {
            totalChance += chance;
        }

        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        for (int i = 0; i < dropItems.Length; i++)
        {
            cumulative += dropChances[i];

            if (roll < cumulative)
            {
                _dropItem.Drop(dropItems[i], _paralaxLayers);
            }
        }
        
        Destroy(gameObject);
    }

    public float GetGroundSpeed()
    {
        return _paralaxLayers.speedRatios[5] * _paralaxLayers.machineSpeed * tankMoveOffset;
    }
    
}

    
