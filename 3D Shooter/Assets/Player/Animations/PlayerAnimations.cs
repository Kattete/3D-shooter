using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    //Walking hashes
    private int walkingHash;
    private int rightStrafeHash;
    private int leftStrafeHash;
    private int walkingBckHash;

    //Running hashes
    private int runningHash;
    private int runningBckHash;

    //Jump Hashes
    private int jumpHash;
    private int jumpBckHash;

    //Crouch Hashes
    private int crouchIdleHash;
    private int crouchWalkHash;
    private void Start()
    {
        animator = GetComponent<Animator>();
        //Walk
        walkingHash = Animator.StringToHash("Walking");
        rightStrafeHash = Animator.StringToHash("RightStrafe");
        leftStrafeHash = Animator.StringToHash("LeftStrafe");
        walkingBckHash = Animator.StringToHash("WalkingBck");
        //Run
        runningHash = Animator.StringToHash("Running");
        runningBckHash = Animator.StringToHash("RunningBck");
        //Jump
        jumpHash = Animator.StringToHash("Jump");
        jumpBckHash = Animator.StringToHash("JumpBck");
        //Crouch
        crouchIdleHash = Animator.StringToHash("CrouchIdle");
        crouchWalkHash = Animator.StringToHash("CrouchWalk");
    }

    private void Update()
    {
        WalkingAnimations();
        RunningAnimations();
        JumpingAnimations();
        CrouchingAnimations();
    }

    private void WalkingAnimations()
    {
        if (Input.GetKey(KeyCode.W)) animator.SetBool(walkingHash, true);
        else animator.SetBool(walkingHash, false);

        if (Input.GetKey(KeyCode.S)) animator.SetBool(walkingBckHash, true);
        else animator.SetBool(walkingBckHash, false);

        if (Input.GetKey(KeyCode.A)) animator.SetBool(leftStrafeHash, true);
        else animator.SetBool(leftStrafeHash, false);

        if (Input.GetKey(KeyCode.D)) animator.SetBool(rightStrafeHash, true);
        else animator.SetBool(rightStrafeHash, false);
    }

    private void RunningAnimations()
    {
        if (Input.GetKey(KeyCode.LeftShift)) animator.SetBool(runningHash, true);
        else animator.SetBool(runningHash, false);
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S)) animator.SetBool(runningBckHash, true);
        else animator.SetBool(runningBckHash, false);
    }

    private void JumpingAnimations()
    {
        if (Input.GetKey(KeyCode.Space)) animator.SetTrigger(jumpHash);
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.S)) animator.SetTrigger(jumpBckHash);
    }

    private void CrouchingAnimations()
    {
        if (Input.GetKey(KeyCode.LeftControl)) animator.SetBool(crouchIdleHash, true);
        else animator.SetBool(crouchIdleHash, false);

        if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) animator.SetBool(crouchWalkHash, true);
        else animator.SetBool(crouchWalkHash, false);
    }
}
