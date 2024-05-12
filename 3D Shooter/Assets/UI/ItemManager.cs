using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image sword;
    [SerializeField] private Image gun;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            sword.enabled = false;
            gun.enabled = false;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            sword.enabled = true;
            gun.enabled = false;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            sword.enabled = false;
            gun.enabled = true;
            gun.gameObject.SetActive(true);
        }

    }

}
