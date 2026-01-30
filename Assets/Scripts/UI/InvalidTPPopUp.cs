using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class InvalidTPPopUp : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.SetText("");
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
        Debug.LogWarning("Teleport UI SHould be here");
        textMeshProUGUI.SetText("Teleport Failed - Space Blocked");
        Debug.Log("Object visible");
        StartCoroutine(PopUpWait());
    }

    private IEnumerator PopUpWait()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Object not visible");
        textMeshProUGUI.SetText("");
    }
}
