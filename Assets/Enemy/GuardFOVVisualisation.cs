using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;

[CustomEditor(typeof(Guard))]
public class GuardFOVVisualisation : Editor
{

    private void OnSceneGUI()
    {
        Guard fov = (Guard)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.Radius);
        
        Vector3 viewAngle1 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.Angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.Angle / 2);
        
        Handles.color = Color.red;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.Radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.Radius);

        if(fov.PlayerDetected)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.PlayerReference.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
