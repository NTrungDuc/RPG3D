using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzelPassword : MonoBehaviour
{
    [SerializeField] public string correctPassword;
    private string enteredPassword = "";
    private string Placeholder = "Enter passcode";
    public Text passwordDisplay;
    private int maxPasswordLength = 4;
    private void OnEnable()
    {
        PlayerMovement.Instance.LockCursor(true, CursorLockMode.None);
    }
    public void AddDigit(string digit)
    {
        if (enteredPassword.Length < maxPasswordLength)
        {
            enteredPassword += digit;
            UpdatePasswordDisplay();
        }
    }

    private void UpdatePasswordDisplay()
    {
        passwordDisplay.text = enteredPassword;
    }
    public void CheckPassword()
    {
        if (enteredPassword == correctPassword)
        {
            Debug.Log("correctPassword");
            InventoryManager.Instance.isWinMiniGame = true;
        }
        else
        {
            Debug.Log("fail");
            enteredPassword = "";
            UpdatePasswordDisplay();
        }
    }
    public void DeleteLastDigit()
    {
        if (enteredPassword.Length > 0)
        {
            enteredPassword = enteredPassword.Substring(0, enteredPassword.Length - 1);
            UpdatePasswordDisplay();
        }
        if(enteredPassword.Length == 0)
        {
            passwordDisplay.text = Placeholder;
        }
    }
    private void OnDisable()
    {
        PlayerMovement.Instance.LockCursor(false, CursorLockMode.Locked);
        enteredPassword = "";
        UpdatePasswordDisplay();
    }
}
