using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;
    public int baseRequiredXP = 100;
    public float xpMultiplier = 1.5f;
    //ui
    public Text textEXP;
    public Image expBar;
    public void GainXP(int xp)
    {
        currentXP += xp;
        
        Debug.Log(CalculateRequiredXP());
        if (currentXP >= CalculateRequiredXP())
        {
            LevelUpPlayer();
        }
        UpdateUI();
    }
    int CalculateRequiredXP()
    {
        return Mathf.RoundToInt(baseRequiredXP * Mathf.Pow(xpMultiplier, level - 1));
    }
    void LevelUpPlayer()
    {
        int requiredXP = CalculateRequiredXP();
        level++;
        currentXP -= requiredXP;
        
        //upgrade hp,skill....

    }
    void UpdateUI()
    {
        expBar.fillAmount = (float) currentXP / (float) CalculateRequiredXP();
        textEXP.text = currentXP.ToString() + "/" + CalculateRequiredXP();
    }
}
