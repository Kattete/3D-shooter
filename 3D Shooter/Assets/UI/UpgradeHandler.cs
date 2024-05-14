using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Update()
    {
        if(Input.GetKey(KeyCode.L)) panel.SetActive(true);
        else panel.SetActive(false);
    }
}
