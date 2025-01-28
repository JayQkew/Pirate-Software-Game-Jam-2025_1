using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth;
    public float moveSpeed;
    public float damage;
    
    
    [Header("Current Stats")]
    public float health;
    public float debuffSpeed;
    
    [Header("References")]
    public SpeedManager _speedManager;
    public ParalaxLayers _paralaxLayers;
    public Rigidbody2D rb;

    public float tankMoveOffset;
    private Color originalColor;

    private void Awake()
    {
        health = maxHealth;
        //currentSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        originalColor = GetComponent<SpriteRenderer>().color;
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
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        debuffSpeed = moveSpeed *.5f;
        yield return new WaitForSeconds(.1f);
        debuffSpeed = 0;
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }
    
}
