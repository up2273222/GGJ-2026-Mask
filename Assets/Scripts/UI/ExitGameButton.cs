using UnityEngine;

public class ExitGameButton : MonoBehaviour, IMenuButton
{
    public void OnClick()
    {
        Debug.Log("exiting game trust me man");
        Application.Quit();
    }
}
