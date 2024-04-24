using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private int walkingHash;
    private int rightStrafeHash;
    private int leftStrafeHash;
    private int walkingBckHash;
    private void Start()
    {
        animator = GetComponent<Animator>();
        walkingHash = Animator.StringToHash("Walking");
        rightStrafeHash = Animator.StringToHash("RightStrafe");
        leftStrafeHash = Animator.StringToHash("LeftStrafe");
        walkingBckHash = Animator.StringToHash("WalkingBck");
    }

    private void Update()
    {
        WalkingAnimations();
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
        if (Input.GetKey(KeyCode.W)) animator.SetBool(walkingHash, true);
        else animator.SetBool(walkingHash, false);

        if (Input.GetKey(KeyCode.S)) animator.SetBool(walkingBckHash, true);
        else animator.SetBool(walkingBckHash, false);

        if (Input.GetKey(KeyCode.A)) animator.SetBool(leftStrafeHash, true);
        else animator.SetBool(leftStrafeHash, false);

        if (Input.GetKey(KeyCode.D)) animator.SetBool(rightStrafeHash, true);
        else animator.SetBool(rightStrafeHash, false);
    }
}
