using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityMultiplier;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump = true;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;


    [SerializeField] private Transform orientation;

    //change to input system
    //add gravity

    float horizontalInput;
    float verticalInput;
    private float gravity = -9.81f;
    private float velocity;
    

    CharacterController cc;
    Vector3 moveDirection;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        ApplyGravity();
        MyInput();
        MovePlayer();
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
