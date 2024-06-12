using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItems : MonoBehaviour
{
    public Items Item;
    private bool isKeyPressed = false;
    public void PickUpItem()
    {
        if (Input.GetKey(KeyCode.F))
        {
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(false);
            if (!isKeyPressed)
            {
                isKeyPressed = true;
                bool result = InventoryManager.Instance.AddItem(Item);
                if (result)
                {
                    Debug.Log("Item Added");
                }
                else
                {
                    Debug.Log("Item not Added");
                }
                GameObject parentObject = transform.parent.gameObject;

                Destroy(parentObject);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(true);
            InventoryManager.Instance.txtPickUps.text = "Press [F] to Pick Up!!";
            PickUpItem();
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
