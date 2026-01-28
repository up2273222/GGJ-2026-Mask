using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [HeaderAttribute("FOV")]

    public float Radius;
    [RangeAttribute(0, 360)]
    public float Angle;

    public GameObject PlayerReference;

    [SerializeField] private LayerMask TargetMask;
    [SerializeField] private LayerMask ObstructionMask;

    public bool PlayerDetected;

    [HeaderAttribute("Patrol")]

    public Transform[] patrolpoints;
    public int targetpoint;
    public float movespeed;
    public float rotationSpeed;
    public GameObject GuardReference;
    
    private void Start()
    {
        //FOV
        PlayerReference = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(GuardFOVRoutine());
        
        //Patrol
        targetpoint = 0;
    }

    private void Update()
    {
        Vector3 targetPos = patrolpoints[targetpoint].position;
        Vector3 direction = (patrolpoints[targetpoint].position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, patrolpoints[targetpoint].position, movespeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                ChangeTargetPoint();
            }
    }

    #region Patrol
    void ChangeTargetPoint()
    {
        targetpoint++;
        if (targetpoint >= patrolpoints.Length)
        {
            targetpoint = 0;
        }
    }
    #endregion

    #region FOVRoutine
    private IEnumerator GuardFOVRoutine()
    {
        Debug.Log("GuardRoutineStarted");
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        Debug.Log("guarding");

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    #endregion

    #region FOV Check
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Radius, TargetMask);

        if (rangeChecks.Length != 0)
        {
            Debug.Log("range");
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
            {
                Debug.Log("angle");
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
                {
                    Debug.Log("player detected");
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
    #endregion
}