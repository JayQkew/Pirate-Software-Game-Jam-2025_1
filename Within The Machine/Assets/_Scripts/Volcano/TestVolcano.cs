using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVolcano : MonoBehaviour
{
    [SerializeField]
    private bool InValcano = false;
    [SerializeField]
    private VolcanoBehavior Volcano;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InValcano)
        {
            Volcano.VolcanoProcess(gameObject);
            InValcano = false;
        }
    }
}
