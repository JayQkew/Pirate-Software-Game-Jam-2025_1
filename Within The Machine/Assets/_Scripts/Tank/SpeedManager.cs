using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    
    public float currentSpeed;
    [Range(0.0f, 5.0f)]
    public float speedSetting;
    public float speedDrag;

    [Header("Visual Movement")]
    public ParalaxLayers paralax;
    public float visualSpeedMultiple;

    [Header("Furnace")]
    public Furnace _furnace;


    private void Update()
    {
        if (!_furnace.isPowering)
        {
            Debug.Log("no power");
            SlowDown();
        }
        else if (speedSetting > currentSpeed)
        {
            SpeedUp();
        }
        else if (currentSpeed > speedSetting)
        {
            SlowDown();
        }

        paralax.machineSpeed = currentSpeed * visualSpeedMultiple;
    }

    void SpeedUp()
    {
        //Debug.Log("Speed up");
        currentSpeed += speedDrag *Time.deltaTime;
        if (currentSpeed > speedSetting) currentSpeed = speedSetting;
    }

    void SlowDown()
    {
        //Debug.Log("Slow");

        float minVal = speedSetting;

        if (!_furnace.isPowering)
        {
            //Debug.Log("power off");
            minVal = 0;
        }

        currentSpeed -= speedDrag * Time.deltaTime;
        if (currentSpeed < minVal) currentSpeed = minVal;
    }


}
