using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Transform linkedDoor;
    IEnumerator OpenDoor(Collider other)
    {
        yield return null;
        InventoryManager.Instance.txtPickUps.gameObject.SetActive(false);

        other.transform.position = linkedDoor.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(true);
            InventoryManager.Instance.txtPickUps.text = "Press [F] to Open!!";
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(OpenDoor(other));
            }
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
