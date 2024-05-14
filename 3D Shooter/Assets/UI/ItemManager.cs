using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image sword;
    [SerializeField] private Image gun;
    [SerializeField] private Image swordSilver;
    [SerializeField] private Image gunSilver;
    [SerializeField] private GameObject ammunitionDisplay;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            sword.enabled = false;
            gun.enabled = false;
            swordSilver.enabled = false;
            gunSilver.enabled = false;
            ammunitionDisplay.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            sword.enabled = true;
            gun.enabled = false;
            gunSilver.enabled = true;
            swordSilver.enabled = false;
            ammunitionDisplay.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            sword.enabled = false;
            gun.enabled = true;
            gun.gameObject.SetActive(true);
            swordSilver.gameObject.SetActive(true);
            swordSilver.enabled = true;
            gunSilver.enabled = false;
            ammunitionDisplay.SetActive(true);
        }

    }

}
