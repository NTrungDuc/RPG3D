using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    private void Awake()
    {
        Deselect();
    }
    public void Select()
    {
        image.color = selectedColor;
        if (gameObject.transform.childCount > 0)
        {
            InventoryItem inventoryItem = gameObject.transform.GetChild(0).GetComponent<InventoryItem>();
            InventoryManager.Instance.ItemInHand[inventoryItem.item.id - 1].SetActive(true);
        }
        else
        {
            foreach (GameObject obj in InventoryManager.Instance.ItemInHand)
            {
                obj.SetActive(false);
            }
        }
    }
    public void Deselect()
    {
        image.color = notSelectedColor;
        if (gameObject.transform.childCount > 0)
        {
            InventoryItem inventoryItem = gameObject.transform.GetChild(0).GetComponent<InventoryItem>();
            InventoryManager.Instance.ItemInHand[inventoryItem.item.id - 1].SetActive(false);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
