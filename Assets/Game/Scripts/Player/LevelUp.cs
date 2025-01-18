using System;
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
    public float currentCoin = 100;
    public Text coin;
    public Text levelPlayer;
    [SerializeField] private ShieldUpgrade shield;
    int levelUpShield = 2;
    public void GainXP(int xp, float index)
    {
        currentXP += xp;
        GetCoin(index);
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
        PlayerMovement.Instance.UpgradeStats(1.2f, 1.1f, level);
        UpShield();
    }
    void UpdateUI()
    {
        expBar.fillAmount = (float) currentXP / (float) CalculateRequiredXP();
        textEXP.text = currentXP.ToString() + "/" + CalculateRequiredXP();
        coin.text = currentCoin.ToString();
        levelPlayer.text = "LV: " + level.ToString();
    }
    void GetCoin(float coinDropped)
    {
        currentCoin = float.Parse(coin.text);
        currentCoin += coinDropped;
    }
    public void SaveDataLevelUp(PlayerData data)
    {
        data.coin = float.Parse(coin.text); 
        data.level = level;
        data.Exp = currentXP;
    }
    public void LoadDataLevelUp(PlayerData data)
    {
        currentCoin = data.coin;
        level = data.level;
        currentXP = data.Exp;
        UpdateUI();
    }
    public void UpShield()
    {
        if (level >= levelUpShield)
        {
            shield.UpgradeShield();
        }
    }
    //cheat
    public void HackEXP()
    {
        currentXP += 100;
        GetCoin(100);
        if (currentXP >= CalculateRequiredXP())
        {
            LevelUpPlayer();
        }
        UpdateUI();
    }
}
