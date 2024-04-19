using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    private bool readyToJump = true;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchYScale;
    [SerializeField] private float startYScale;


    [SerializeField] private Transform orientation;

    //change to input system
    //add gravity

    float horizontalInput;
    float verticalInput;
    private float gravity = -9.81f;
    private float velocity;

    private Controlls cl;
    CharacterController cc;
    Vector3 moveDirection;

    public MovementState state;
    public bool freeze;
    public bool activeGrapple;
    private Vector3 velocityToSet;

    private bool enableMovementOnNextTouch;

    public enum MovementState
    {
        freeze,
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        cl = new Controlls();
        cl.Enable();
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        ApplyGravity();
        MyInput();
        MovePlayer();
        stateHandle();
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
        Invoke(nameof(ResetRestricion), 3f);
    }

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        cc.Move(velocityToSet);
    }

    public void ResetRestricion()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestricion();
            GetComponent<Grappling>().StopGrapple();
        }
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(RedyJump), jumpCooldown);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void stateHandle()
    {
        //freeze mode

        if (freeze)
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            gravity = 0;
        }

        else if (Input.GetKey(KeyCode.LeftControl))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            gravity = -9.81f;
        }

        //sprinting state
        if(grounded && Input.GetKey(KeyCode.LeftShift))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            gravity = -9.81f;
        }else if (grounded) //walking state
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            gravity = -9.81f;
        }
        else //air state
        {
            state = MovementState.air;
            gravity = -9.81f;
        }
    }

    private void MovePlayer()
    {
        if (activeGrapple) return;  

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        cc.Move(moveDirection * moveSpeed);
    }

    private void Jump()
    {
        cc.Move(transform.up * jumpForce);

    }

    private void RedyJump()
    {
        readyToJump = true;
    }

    private void ApplyGravity()
    {

        if (grounded && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;

        }
        moveDirection.y = velocity;
        cc.Move(moveDirection * Time.deltaTime);
    }

    private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajecctoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajecctoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajecctoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY  - trajecctoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}
