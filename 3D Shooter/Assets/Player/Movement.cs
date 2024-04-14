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

    public enum MovementState
    {
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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        //sprinting state
        if(grounded && Input.GetKey(KeyCode.LeftShift))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }else if (grounded) //walking state
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else //air state
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
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
}
