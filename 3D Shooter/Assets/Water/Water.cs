using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<Movement>() != null)
        {
            Movement movement = other.GetComponent<Movement>();
            movement.isSwimming = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Movement>() != null)
        {
            Movement movement = other.GetComponent<Movement>();
            movement.isSwimming = false;
        }
    }
}
