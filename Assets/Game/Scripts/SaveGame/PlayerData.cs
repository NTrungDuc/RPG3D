using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int level;
    public int levelShield;
    public int Exp;
    public float health;
    public float stamina;
    public float coin;
    public List<Items> items;
    public List<string> itemsDestroyed;
}
