using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    [Header("Particles")] 
    [SerializeField] private ParticleSystem MoveParticles;
    [SerializeField] private ParticleSystem FallParticles;
    [SerializeField] private ParticleSystem DashLeftParticles;

    [Range(0,1)]
    [SerializeField] private float spawnMoveDustPeriod; //used to control how frequent the movement particles need to spawn.

    [SerializeField] public bool isRunning;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
           timer += Time.deltaTime;
           if (timer > spawnMoveDustPeriod)
           {
               MoveParticles.Play();
               timer = 0;
           }
           
        }  
    }

    public void Landed()
    {
        FallParticles.Play();
    }

    public void DashLeft()
    {
        DashLeftParticles.Play();
    }
}
