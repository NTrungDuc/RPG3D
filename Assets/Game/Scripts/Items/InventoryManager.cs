using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public static InventoryManager Instance { get { return instance; } }
    public int maxStackItem = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    int selectedSlot = -1;
    [SerializeField] public Text txtPickUps;
    [SerializeField] public GameObject[] ItemInHand;
    public bool isWinMiniGame = false;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        inputSelectItem();
    }
    public void inputSelectItem()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 7)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
        if (selectedSlot != -1)
        {
            GetItemSelected();
        }
    }
    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    public bool AddItem(Items item)
    {
        SaveItem(item);
        //count
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackItem && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        //find empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }
    void SpawnNewItem(Items item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public void GetItemSelected()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && !PlayerMovement.Instance.isOpenInventory)
        {
            UseItem useItem = ItemInHand[itemInSlot.item.id - 1].GetComponent<UseItem>();
            useItem.Use();
            if(useItem.hasSkill)
            {
                WeaponsCDManager.Instance.UpdateUICooldown(Vector3.one, Mathf.CeilToInt(useItem.value), true);
            }
            else
            {
                WeaponsCDManager.Instance.UpdateUICooldown(Vector3.zero, -1, false);
            }
        }else if(itemInSlot == null)
        {
            WeaponsCDManager.Instance.UpdateUICooldown(Vector3.zero, -1, false);
        }
    }
    public void refreshCountItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        itemInSlot.count--;
        if (itemInSlot.count <= 0)
        {
            Destroy(itemInSlot.gameObject);
            ItemInHand[itemInSlot.item.id - 1].SetActive(false);
        }
        else
        {
            itemInSlot.RefreshCount();
        }

    }
    void SaveItem(Items item)
    {
        GameManager.Instance.allItemsInventory.Add(item);
    }
}
