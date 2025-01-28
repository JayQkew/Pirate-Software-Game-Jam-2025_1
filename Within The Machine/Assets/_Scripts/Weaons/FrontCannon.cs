using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCannon : Weapon
{
    public GameObject targetEnemy;
    

    public override bool FireWeapon()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right);
        
        if (hit.collider == null) return false;
        if (!hit.collider.CompareTag("Enemy")) return false;
        
        Debug.Log("Fire!!");
        hit.collider.GetComponent<Enemy>().TakeDamage(damage);
         return true;
    }
}
