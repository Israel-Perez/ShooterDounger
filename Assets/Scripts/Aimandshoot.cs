using System;
using UnityEngine;

public class Aimandshoot : MonoBehaviour
{
    [SerializeField] private Transform playertransform;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = playertransform.position;
        
    }
}
