using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItems : MonoBehaviour
{
    [SerializeField] private GameObject selectUI;
    public Items Item;
    public void PickUpItem()
    {
        if (Input.GetKey(KeyCode.F))
        {
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
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(true);
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
