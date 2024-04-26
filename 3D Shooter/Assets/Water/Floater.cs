using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private Rigidbody rb;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    Movement movement;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
    }

    private void FixedUpdate()
    {
        if(movement.isSwimming)
        {
            float displacementMultiplier = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f));
        }
    }
}
