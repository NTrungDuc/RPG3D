using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : MonoBehaviour
{
    int shieldLevel = 1;
    public float shieldValue;
    [SerializeField] private GameObject[] shieldPrefabs;
    private GameObject currentShield;
    private void Start()
    {
        if (shieldPrefabs.Length > 0)
        {
            EquipShield(shieldLevel);
        }
    }
    public void UpgradeShield()
    {
        if (shieldLevel <= shieldPrefabs.Length - 1)
        {
            shieldLevel++;
            float newShieldValue = shieldValue * Mathf.Pow(1.3f, shieldLevel);
            shieldValue = newShieldValue;
            EquipShield(shieldLevel);
            Debug.Log("Shield upgraded to level " + shieldLevel);
        }
        else
        {
            Debug.Log("Shield is already at maximum level");
        }
    }

    private void EquipShield(int shieldLevel)
    {
        if(currentShield != null)
        {
            currentShield.SetActive(false);
        }
        currentShield = shieldPrefabs[shieldLevel - 1];
        currentShield.SetActive(true);
    }
}
