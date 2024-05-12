using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindManger : MonoBehaviour
{
    [SerializeField] private GameObject keyBindPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) keyBindPanel.SetActive(true);
        else if (Input.GetKeyUp(KeyCode.Escape)) keyBindPanel.SetActive(false);
    }
}
