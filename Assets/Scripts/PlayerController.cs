using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private BroadCasterClass levelChangeManager;
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
        float sphereCastRadius = capsuleCollider.radius - 0.2f;
        Vector3 sphereCastOrigin = groundCheck.position;
        isGrounded = Physics.SphereCast(sphereCastOrigin, sphereCastRadius, Vector3.down, out RaycastHit hit, 1f);
        
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
        rb.isKinematic = true;
        if (newState == WorldState.Comedy)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y + levelOffset, transform.position.z);
            //transform.position = new Vector3(transform.position.x, 41f, transform.position.z);
            rb.position = new Vector3(transform.position.x, transform.position.y + levelOffset + 0.5f, transform.position.z);
            Debug.Log("Teleported Up to Comedy");
        }
        else
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y - levelOffset, transform.position.z);
            //transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
            rb.position = new Vector3(transform.position.x, transform.position.y - levelOffset + 0.5f, transform.position.z);
            Debug.Log("Teleported Down to Tragedy");
        }

        rb.isKinematic = false;

    }
}
