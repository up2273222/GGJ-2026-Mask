using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class InvalidTPPopUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GameObject().SetActive(false);
    }

    private void OnEnable()
    {
        PlayerController.TeleportFailed += TPFailedPopUP;
    }

    private void OnDisable()
    {
        PlayerController.TeleportFailed -= TPFailedPopUP;
    }

    // Update is called once per frame

    private void TPFailedPopUP()
    {
        this.GameObject().SetActive(true);
        StartCoroutine(PopUpWait());
        this.GameObject().SetActive(false);

    }

    private IEnumerator PopUpWait()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
