using System;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private BoxCollider boxCollider;

    [SerializeField] private GameObject winMenu;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            winMenu.gameObject.SetActive(true);
        }
    }
}
