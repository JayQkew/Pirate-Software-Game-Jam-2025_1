using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Explosion : MonoBehaviour
{
    public float damage;
    public float maxExplosionRadius;
    public float duration;
    private float elapsedTime;

    public void Explode(float damage, float maxRadius)
    {
        this.damage = damage;
        maxExplosionRadius = maxRadius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }    
    }
    

    private void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolate between the initial and target scale
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * maxExplosionRadius, elapsedTime / duration);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
