using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float mySpeed;
    [SerializeField] private float groundDrag; //friction
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Ground Check")]
    [SerializeField]private float playerHeight;
    [SerializeField] private LayerMask isGround;
    private bool grounded;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 myDirection;
    private Rigidbody myRigidbody;
    public Transform orientation;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, isGround);

        MovementInputControl();
        SpeedControl();

        // handle drag
        if (grounded) myRigidbody.drag = groundDrag;
        else myRigidbody.drag = 0;
    }

    private void FixedUpdate()
    {
        PlayerMovementControl();
    }

    private void MovementInputControl()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void PlayerMovementControl()
    {
        // calculate movement direction
        myDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground or air
        if (grounded) myRigidbody.AddForce(myDirection.normalized * mySpeed * 10f, ForceMode.Force);
        else if (!grounded) myRigidbody.AddForce(myDirection.normalized * mySpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 velocity = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z);

        // limit velocity if needed
        if (velocity.magnitude > mySpeed)
        {
            Vector3 limitedVel = velocity.normalized * mySpeed;
            myRigidbody.velocity = new Vector3(limitedVel.x, myRigidbody.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0f, myRigidbody.velocity.z);

        myRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
