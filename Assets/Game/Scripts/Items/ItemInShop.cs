using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInShop : MonoBehaviour
{
    public Items Item;
    [SerializeField] private Text money;
    public void buyItem()
    {
        float currentMoney = float.Parse(money.text);
        if (currentMoney >= Item.price)
        {
            //Debug.Log("buy");
            currentMoney -= Item.price;
            money.text = currentMoney.ToString();
            InventoryManager.Instance.AddItem(Item);
        }
    }
    private void OnDisable()
    {
        PlayerMovement.Instance.LockCursor(false, CursorLockMode.Locked);
    }
}
