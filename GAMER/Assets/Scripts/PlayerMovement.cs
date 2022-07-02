using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    float movementMultiplier = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.2f)) {
            if (slopeHit.normal != Vector3.up) {
                return true;
            } else {
                return false;
            }
        }
        return false;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

        private void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();

        if (Input.GetKeyDown(jumpKey) && isGrounded) { 
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void MyInput() {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    private void Jump() {
        if (isGrounded) {
            //rb.velocity = new Vector3(rb.velocity.x, -jumpForce, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MovePlayer() {
        if (isGrounded && !OnSlope()) {
            rb.velocity = moveDirection.normalized * moveSpeed * movementMultiplier;
        } else if (isGrounded && OnSlope()) {
            rb.velocity = slopeMoveDirection.normalized * moveSpeed * movementMultiplier;
        } else if (!isGrounded) {
            //rb.velocity = new Vector3(rb.velocity.x, -gravity, rb.velocity.z);
            rb.velocity = moveDirection.normalized * moveSpeed * movementMultiplier;
        }

        // if (isGrounded && !OnSlope())
        // {
        //     rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        // }
        // else if (isGrounded && OnSlope())
        // {
        //     rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        // }
        // else if (!isGrounded)
        // {
        //     rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        // }
    }
}