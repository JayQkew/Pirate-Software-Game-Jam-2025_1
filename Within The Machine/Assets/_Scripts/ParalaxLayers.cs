using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ParalaxLayers : MonoBehaviour
{
    public GameObject[] layers;
    private Material[] materials;
    private float[] distances;
    public float[] speedRatios;
    public float machineSpeed;

    private void Start()
    {
        materials = new Material[layers.Length];
        distances = new float[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            materials[i] = layers[i].GetComponent<Renderer>().material;
        }
    }

    private void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            distances[i] += Time.deltaTime * speedRatios[i] * machineSpeed;
            materials[i].SetTextureOffset("_MainTex", Vector2.right * distances[i]);
        }
    }
}
