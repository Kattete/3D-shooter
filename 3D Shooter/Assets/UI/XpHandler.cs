using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text xpText;
    [Header("XP")]
    [SerializeField]private float requiredXp = 100f;
    private int[] xpPerLevel;
    public int lvl;
    private int currentXp;

    private void Start()
    {
        lvl = 1;
        currentXp = 0;
    }

    private void Update()
    {
        xpSlider.value = currentXp;
    }

    public void AddXP(int xpToAdd)
    {
        currentXp += xpToAdd;
        CheckForLevelUp();
    }

    void CheckForLevelUp()
    {
        if(currentXp >= requiredXp)
        {
            lvl++;
        }
    }

    }
