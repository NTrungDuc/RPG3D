using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public ChatNPC chat;
    [SerializeField] private GameObject ConversationUI;
    [SerializeField] private GameObject Shop;
    [SerializeField] private Text boxChat;
    [SerializeField] private Button[] option;
    private int currentLine = 1;
    private bool isKeyPressed = false;
    bool isTalk = false;
    private void Start()
    {
        for (int i = 0; i < option.Length; i++)
        {
            int index = i;
            option[i].onClick.AddListener(() => GetValueSelectOption(index));
        }
    }
    public void Talk()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isTalk = true;
            PlayerMovement.Instance.LockCursor(true, CursorLockMode.None);
            ConversationUI.SetActive(true);
            boxChat.text = chat.dialogText[0];
        }
        if (Input.GetMouseButton(0) && isTalk)
        {
            if (!isKeyPressed)
            {
                isKeyPressed = true;
                if (currentLine < chat.dialogText.Length)
                {
                    boxChat.text = chat.dialogText[currentLine];
                    currentLine++;
                }
                else
                {
                    if (chat.options != null)
                    {
                        for (int i = 0; i < chat.options.Length; i++)
                        {
                            option[i].gameObject.SetActive(true);
                            Text txt = option[i].transform.GetChild(0).GetComponent<Text>();
                            txt.text = chat.options[i];
                        }
                    }
                    else
                    {
                        ResetValue();
                    }
                }
            }
        }
        else
        {
            isKeyPressed = false;
        }
    }
    public void GetValueSelectOption(int index)
    {
        chat.selectedOption = index;
        selectedOption();
    }
    public void selectedOption()
    {
        if (chat.selectedOption != -1)
        {
            switch (chat.selectedOption)
            {
                case 0:
                    Case0();
                    break;
                case 1:
                    Case1();
                    break;
            }

            chat.selectedOption = -1;
        }
    }
    void Case0()
    {
        if (chat.speaker == TypeNPC.SalesMan)
        {
            //Open Shop
            Shop.gameObject.SetActive(true);
            ResetValue();
        }
    }
    void Case1()
    {
        if (chat.speaker == TypeNPC.SalesMan)
        {
            ResetValue();
            PlayerMovement.Instance.LockCursor(false, CursorLockMode.Locked);
        }
    }
    public void ResetValue()
    {
        isKeyPressed = false;
        isTalk=false;
        currentLine = 1;
        ConversationUI.SetActive(false);
        foreach (Button b in option)
        {
            b.gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(true);
            InventoryManager.Instance.txtPickUps.text = "Press [F] to Talk NPC!!";
            Talk();
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
