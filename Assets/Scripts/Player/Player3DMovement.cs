using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float mySpeed;
    [SerializeField] private float groundDrag; //friction
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private Vector3 myDirection;
    private bool readyToJump;

    // Camera control using mouse
    private float horizontalInput;
    private float verticalInput;
    private float mouseSensitivity = 100f;
    private float xRotation = 0f;
    public Transform cameraTransform;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask isGround;
    private bool grounded;

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

        Debug.Log("ground" + grounded);
    }

    private void FixedUpdate()
    {
        PlayerMovementControl();
        CameraControl();
    }

    private void MovementInputControl()
    {
        // Get input from W,A,S,D keys
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

    private void CameraControl()
    {
        if (Input.GetKey(KeyCode.W) && grounded)
        {
            // calculate target position to move the camera towards
            Vector3 targetPosition = transform.position + (orientation.forward * 3f);

            // lerp the camera position towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
        }
        else
        {
            // calculate camera rotation based on mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // limit vertical rotation to avoid over-rotation
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // apply camera rotation
            orientation.Rotate(Vector3.up * mouseX);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
