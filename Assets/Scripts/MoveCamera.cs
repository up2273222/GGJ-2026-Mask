using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    void FixedUpdate()
    {
        transform.position = cameraTransform.position;
    }
}
