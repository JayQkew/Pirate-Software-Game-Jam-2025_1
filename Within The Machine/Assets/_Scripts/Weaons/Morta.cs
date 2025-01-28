using UnityEngine;

public class WeaponController : Weapon
{
    public GameObject projectilePrefab;  // The projectile prefab
    public Transform firePoint;          // The point where the projectile is fired
    public float arcDuration = 1.5f;     // Time it takes for the projectile to hit the ground


    public override bool FireWeapon()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy == null)
        {
            return false;
        }
        
        Debug.Log("Launching Morta!!");
        
        FireProjectile(closestEnemy.transform.position);


        return true;
    }
    
    
    public void FireAtClosestEnemy()
    {
        // Find the closest enemy
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            FireProjectile(closestEnemy.transform.position);
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Tag enemies with "Enemy"
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void FireProjectile(Vector3 targetPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        MortaProjectile projectileScript = projectile.GetComponent<MortaProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Launch(firePoint.position, targetPosition, arcDuration,damage);
        }
    }
}