using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] private float baseWalkSpeed;
    [SerializeField] public float sprintSpeed;
    [SerializeField] private float baseSprintSpeed;
    [SerializeField] private Transform orientation;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [Header("GroundCheck")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;
    private bool readyToJump = true;
    [Header("Crouching")]
    [SerializeField] public float crouchSpeed;
    [SerializeField] private float baseCrouchSpeed;
    [SerializeField] private float crouchYscale;
    private float startYScale;
    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    [Header("Swimming")]
    public bool isSwimming;
    [SerializeField] private float swimSpeed;
    [Header("Change Weapon")]
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject gun;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    private bool exitingSlope;
    public bool freeze;
    public bool activeGrapple;

    private bool enableMovementOnNextTouch;

    public MovemenState state;
    public enum MovemenState
    {
        swimming,
        freeze,
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
        walkSpeed = baseWalkSpeed;
        sprintSpeed = baseSprintSpeed;
        crouchSpeed = baseCrouchSpeed;
    }

    private void Update()
    {
        //groudn check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedConrol();
        StateHandler();
        ChangeWeapon();

        if (grounded && !activeGrapple) rb.drag = groundDrag;
        else if (isSwimming == true) rb.drag = 10f;
        else rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //start crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYscale, transform.localScale.z);
        }

        //stop crouch
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        //Mode - swimming
        if (isSwimming)
        {
            state = MovemenState.swimming;
            moveSpeed = swimSpeed;
            rb.useGravity = false;
        }

        //Mode - Freeze
        if (freeze)
        {
            state = MovemenState.freeze;
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }

        //Mode - Crouching
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            state = MovemenState.crouching;
            moveSpeed = crouchSpeed;
            rb.useGravity = true;
        }

        //Mode - sprinting
        else if(grounded && Input.GetKey(KeyCode.LeftShift))
        {
            state = MovemenState.sprinting;
            moveSpeed = sprintSpeed;
            rb.useGravity = true;
        }

        //Mode - walking
        else if (grounded)
        {
            state = MovemenState.walking;
            moveSpeed = walkSpeed;
            rb.useGravity = true;
        }

        //Mode - air
        else if(isSwimming != true)
        {
            state = MovemenState.air;
            rb.useGravity = true;
        }
    }

    private void MovePlayer()
    {
        if (activeGrapple) return;
        //calculate move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
        }


        if(grounded) rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded) rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedConrol()
    {
        if (activeGrapple) return;
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endpoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endpoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endpoint.x - startPoint.x, 0f, endpoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
        Invoke(nameof(ResetRestriction), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }

    public void ResetRestriction()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestriction();
            GetComponent<Grappling>().StopGrapple();
        }
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            sword.SetActive(false);
            gun.SetActive(false);
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            sword.SetActive(true);
            gun.SetActive(false);
        }else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            sword.SetActive(false);
            gun.SetActive(true);
        }
    }
}
