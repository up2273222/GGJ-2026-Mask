using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    
    [SerializeField] private float moveSpeed;
    private float inputX;
    private float inputY;
    private Vector3 moveDirection;
    [SerializeField] private float dragAmount;

    [SerializeField] private float jumpForce;
    private float jumpCooldown;
    private float jumpTimer;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;
    
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

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
        jumpTimer -= Time.deltaTime;
        MovePlayer();
        if (isGrounded && Input.GetKey(KeyCode.Space) && jumpTimer <= 0)
        {
            jumpTimer = jumpCooldown;
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
        
        rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
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
}
