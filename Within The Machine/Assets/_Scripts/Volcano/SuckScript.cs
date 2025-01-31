using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckScript : MonoBehaviour
{
  [SerializeField] private VolcanoBehavior volcano;
  [SerializeField] private Transform EndPosition;
  [SerializeField] private float SetSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<TestVolcano>() != null)
        {
            TestVolcano Tvolcano = other.gameObject.GetComponent<TestVolcano>();
            Tvolcano.Volcano = volcano;
            Tvolcano.Target = EndPosition;
            Tvolcano.speed = SetSpeed;
            Tvolcano.GetSuckedUp();
        }
    }
}
