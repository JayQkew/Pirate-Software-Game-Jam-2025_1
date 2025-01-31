using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutIn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<TestVolcano>() != null)
        {
            TestVolcano Tvolcano = other.gameObject.GetComponent<TestVolcano>();
            Tvolcano.PutInVolcano();
        }
    }
}
