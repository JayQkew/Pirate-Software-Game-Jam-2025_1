using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FrontCannon : Weapon
{
    
    
    //public GameObject targetEnemy;
    

    // public override bool FireWeapon()
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right);
    //     
    //     if (hit.collider == null) return false;
    //     if (!hit.collider.CompareTag("Enemy")) return false;
    //     
    //     Debug.Log("Fire!!");
    //     hit.collider.GetComponent<Enemy>().TakeDamage(damage);
    //      return true;
    // }
    
    public override bool FireWeapon()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy == null)
        {
            return false;
        }
        
        Debug.Log("fire cannon!!");
        
        HitEnemy(closestEnemy);


        return true;
    }
    
    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Tag enemies with "Enemy"
        GameObject closest = null;
        float shortestDistance = range;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= shortestDistance)
            {
                shortestDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }
    
    
    public void HitEnemy(GameObject target )
    {
        target.GetComponent<Enemy>().TakeDamage(damage);
        //RuntimeManager.PlayOneShot(fireSound);
    }
}
