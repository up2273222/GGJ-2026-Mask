using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GuardFOV : MonoBehaviour
{
    public float Radius;
    [RangeAttribute(0, 360)]
    public float Angle;

    public GameObject PlayerReference;

    [SerializeField] private LayerMask TargetMask;
    [SerializeField] private LayerMask ObstructionMask;

    public bool PlayerDetected;

    private void Start()
    {
        PlayerReference = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(GuardFOVRoutine());
    }


    private IEnumerator GuardFOVRoutine()
    {
        Debug.Log("GuardRoutineStarted");
        WaitForSeconds wait = new WaitForSeconds(0.2f);    

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Radius, TargetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.forward, directionToTarget, distanceToTarget, ObstructionMask))
                {
                    PlayerDetected = true;
                }
                else
                {
                    PlayerDetected = false;
                }
            }
            else
            {
                PlayerDetected = false;
            }
        }
        else if(PlayerDetected)
        {
            PlayerDetected = false;
        }
    }
}