using System;
using UnityEngine;

public class MortaProjectile : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float launchSpeed;
    private Vector3 velocity;
    private bool isLaunched = false;
    private float damage;
    
    public float explodeAtY = 0f; // Set the Y position at which the projectile explodes
    

    public void Launch(Vector3 start, Vector3 target, float duration, float damage)
    {
        this.damage = damage;
        startPosition = start;
        targetPosition = target;

        // Calculate the initial velocity needed to hit the target within the given duration
        Vector3 displacement = target - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);
        float horizontalDistance = displacementXZ.magnitude;

        float verticalDistance = displacement.y;
        float gravity = Mathf.Abs(Physics.gravity.y);

        // Initial velocity components
        float verticalVelocity = (verticalDistance + 0.5f * gravity * Mathf.Pow(duration, 2)) / duration;
        float horizontalVelocity = horizontalDistance / duration;

        // Compute the velocity vector
        velocity = displacementXZ.normalized * horizontalVelocity + Vector3.up * verticalVelocity;

        isLaunched = true;
    }

    private void Update()
    {
        if (isLaunched)
        {
            // Apply motion using velocity and gravity
            transform.position += velocity * Time.deltaTime;
            velocity += Physics.gravity * Time.deltaTime;

            // Check if we've reached (or passed) the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Hit the target
                Destroy(gameObject);
            }
        }
        
        if (transform.position.y <= explodeAtY)
        {
            Explode();
        }
    }
    
    private void Explode()
    {
        Debug.Log("Explode");
        Destroy(gameObject);
    }
}