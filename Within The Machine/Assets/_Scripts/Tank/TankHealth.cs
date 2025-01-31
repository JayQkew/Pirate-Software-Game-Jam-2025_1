using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    
    [SerializeField] float currentHealth;
    
    [SerializeField] TMP_Text tankStatusText;

    private void Awake()
    {
        currentHealth = maxHealth;
        HealthStatus();
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }

        HealthStatus();
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        HealthStatus();
    }

    void Die()
    {
        Debug.Log("LOSE THE GAME");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            TakeDamage(enemy.damage);
            enemy.TakeDamage(enemy.maxHealth);
        }
    }

    void HealthStatus()
    {
        tankStatusText.text = $"{currentHealth}%";

        if (currentHealth < 15)
            tankStatusText.text = "CRITICAL!";

    }
}
