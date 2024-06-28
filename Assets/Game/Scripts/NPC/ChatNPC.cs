using UnityEngine;
[CreateAssetMenu(fileName = "New Chat", menuName = "NPC/Create New Item")]
public class ChatNPC : ScriptableObject
{
    public TypeNPC speaker;
    public string[] dialogText;
    public bool isPlayerDialog;
    public int currentLine = 0;
    public string[] options;
    public int selectedOption = -1;
}
public enum TypeNPC
{
    SalesMan,
    localPeople
}
