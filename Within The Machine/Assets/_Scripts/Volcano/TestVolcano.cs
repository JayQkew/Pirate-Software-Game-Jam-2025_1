using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestVolcano : MonoBehaviour
{
    [SerializeField]
    private bool InVolcano = false;
    public VolcanoBehavior Volcano;
    [SerializeField] private bool isFloating = false;
    public Transform Target;
    public float speed;
   
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFloating)
        {
            transform.position = Vector2.Lerp(transform.position, Target.position, speed * Time.deltaTime);
        }
    }

    public void PutInVolcano()
    {
        isFloating = false;
        Volcano.VolcanoProcess(gameObject);
        InVolcano = false;
    }


    public void GetSuckedUp()
    {
        isFloating = true;
    }
}
