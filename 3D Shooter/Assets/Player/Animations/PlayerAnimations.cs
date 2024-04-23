using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void WalkingAnimations()
    {
        //https://www.youtube.com/watch?v=m8rGyoStfgQ&ab_channel=iHeartGameDev
    }

    private void RunningAnimations()
    {

    }
}
