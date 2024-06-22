using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private Items Item;
    [SerializeField] private GameObject miniGame;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider Collider;
    public ChestType chestType;
    [Header("Puzzel Password")]
    [SerializeField] private string Password;
    IEnumerator Open()
    {
        if (Input.GetKey(KeyCode.F))
        {
            miniGame.SetActive(true);
            if (chestType == ChestType.PuzzelPassword)
            {
                miniGame.GetComponent<PuzzelPassword>().correctPassword = Password;
            }
        }
        if (InventoryManager.Instance.isWinMiniGame)
        {
            //open chest
            miniGame.SetActive(false);
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(false);
            animator.SetBool(Constant.OPEN_CHEST, true);
            InventoryManager.Instance.isWinMiniGame = false;

            //add item
            bool result = InventoryManager.Instance.AddItem(Item);
            if (result)
            {
                Debug.Log("Collected Item");
                Collider.enabled = false;
            }
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(true);
            InventoryManager.Instance.txtPickUps.text = "Press [F] to Open!!";
            StartCoroutine(Open());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(false);
        }
    }
}
public enum ChestType
{
    MiniGame,
    PuzzelPassword
}
