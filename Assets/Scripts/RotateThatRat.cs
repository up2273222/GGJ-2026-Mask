using System;
using UnityEngine;

public class RotateThatRat : MonoBehaviour
{
    [SerializeField] private Transform thatRat;
    [SerializeField] private Transform cameraPos;
    

    void Update()
    {
        thatRat.rotation = cameraPos.rotation;
    }
}
