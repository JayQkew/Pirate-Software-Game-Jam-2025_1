using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth;
    public float speed;
    public float damage;
    
    
    [Header("Current Stats")]
    public float health;
    public float currentSpeed;
    
    [Header("References")]
    public SpeedManager _speedManager;
    public ParalaxLayers _paralaxLayers;
    public Rigidbody2D rb;

    private void Awake()
    {
        health = maxHealth;
        currentSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(-currentSpeed - _paralaxLayers.speedRatios[5] * _paralaxLayers.machineSpeed, 0);
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
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        currentSpeed *= .5f;
        yield return new WaitForSeconds(.1f);
        currentSpeed = speed;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Die()
    {
        Destroy(gameObject);
    }
    
}
