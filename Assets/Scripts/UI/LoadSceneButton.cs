using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadSceneButton : MonoBehaviour, IMenuButton
{
    [SerializeField] private String sceneName;

    public void OnClick()
    {
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
            Debug.Log("loading scene" + sceneName  +"trust me man");
        }
        else
        {
            Debug.Log("no scene name provided ");
        }
        
    }
}

