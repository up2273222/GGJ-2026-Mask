using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] Animator ratAnimator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float animatorspeed;

    [HeaderAttribute("FOV")]

    public float Radius;
    [RangeAttribute(0, 360)]
    public float Angle;

    public GameObject PlayerReference;

    [SerializeField] private LayerMask TargetMask;
    [SerializeField] private LayerMask ObstructionMask;

    public bool PlayerDetected;

    [HeaderAttribute("Patrol")]

    [SerializeField] Transform[] patrolpoints;
    public int targetpoint;
    public float movespeed;
    public float rotationSpeed;
    
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
        ratAnimator.SetFloat("Speed", animatorspeed);

        if (patrolpoints.Length > 1)
        {
            Vector3 targetPos = patrolpoints[targetpoint].position;
            Vector3 direction = (patrolpoints[targetpoint].position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            if (animatorspeed != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, patrolpoints[targetpoint].position, movespeed * Time.deltaTime);
            }
           
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                ChangeTargetPoint();
            } 
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
        WaitForSeconds wait = new WaitForSeconds(0.2f);

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
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
                {
                    Debug.Log("Player Detected");
                    PlayerDetected = true;
                }
            }
        }
    }
    #endregion
}