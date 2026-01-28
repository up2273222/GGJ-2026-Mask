using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private LevelSwapManager levelChangeManager;
    [SerializeField] private float moveSpeed;
    private float inputX;
    private float inputY;
    private Vector3 moveDirection;
    [SerializeField] private float dragAmount;

    [SerializeField] private float jumpForce;
    private float jumpCooldown;
    private float jumpTimer;
    private bool jumpPressed;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;

    [SerializeField] private float levelOffset;
    
    [SerializeField] private Mesh playerMesh;
    
    private bool justSwitched;
    
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    
    void OnEnable()
    {
        //levelChangeManager.AddObserver(this);
        LevelSwapManager.OnLevelChanged += OnLevelChange;
    }

    void OnDisable()
    {
        //levelChangeManager.RemoveObserver(this);
        LevelSwapManager.OnLevelChanged -= OnLevelChange;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        rb.freezeRotation = true;
        rb.linearDamping = dragAmount;
    }

    void Update()
    {
        Inputs();
        GroundCheck();
        
    }
    
    void FixedUpdate()
    {
        if (justSwitched)
        {
            justSwitched = false;
            return;
        }
        jumpTimer -= Time.fixedDeltaTime;
        MovePlayer();
        if (isGrounded && Input.GetKey(KeyCode.Space) && jumpTimer <= 0)
        {
            jumpTimer = jumpCooldown;
            //jumpPressed = false;
            Jump();
        }
    }
    private void Inputs()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * inputY + orientation.right * inputX;
        
        float currentSpeed = rb.linearVelocity.magnitude;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * 0.45f), ForceMode.Force);
        }
        
        SpeedControl();
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0 , rb.linearVelocity.z);
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 newVelocity = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);
        }
    }

    private void GroundCheck()
    {
        float sphereCastRadius = 1.0f;
        Vector3 sphereCastOrigin = groundCheck.position;
        isGrounded = Physics.SphereCast(sphereCastOrigin, sphereCastRadius, Vector3.down, out RaycastHit hit, 0.25f);
        
        if (isGrounded)
        {
            rb.linearDamping = dragAmount;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0 , rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void OnLevelChange(WorldState newState)
    {
        
        if (newState == WorldState.Comedy)
        {
            bool isTeleportAreaBlocked = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + levelOffset + 1.0f, transform.position.z), new Vector3(0.7f, 0.15f, 0.7f), Quaternion.identity).Length > 0;
            if (isTeleportAreaBlocked)
            {
                Debug.LogWarning("Area Not Viable for teleport");
                levelChangeManager.LevelSwitchSuccessful(!isTeleportAreaBlocked);
                return;
            }
            levelChangeManager.LevelSwitchSuccessful(!isTeleportAreaBlocked);
            StartCoroutine(WaitTeleportComedy(1.5f));
            
        }
        else
        {
            bool isTeleportAreaBlocked = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y - levelOffset + 1.0f, transform.position.z), new Vector3(0.7f, 0.15f, 0.7f), Quaternion.identity).Length > 0;
            if (isTeleportAreaBlocked)
            {
                Debug.LogWarning("Area Not Viable for teleport");
                levelChangeManager.LevelSwitchSuccessful(!isTeleportAreaBlocked);
                return;
            }
            levelChangeManager.LevelSwitchSuccessful(!isTeleportAreaBlocked);
            StartCoroutine(WaitTeleportTragedy(1.5f));
            
        }
        
        justSwitched = true;
    }

    private IEnumerator WaitTeleportComedy(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb.position = new Vector3(transform.position.x, transform.position.y + levelOffset, transform.position.z);
            
        Debug.Log("Teleported Up to Comedy");
    }

    private IEnumerator WaitTeleportTragedy(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb.position = new Vector3(transform.position.x, transform.position.y - levelOffset, transform.position.z);
            
        Debug.Log("Teleported Down to Tragedy");
    }
    
}
