using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Item/Create New Item")]
public class Items : ScriptableObject
{
    public int id;
    public string itemName;
    public Type type;
    public int value;
    public Sprite icon;
}
public enum Type
{
    Weapon,
    Positon,
    Amo
}
