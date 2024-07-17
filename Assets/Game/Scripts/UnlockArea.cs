using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockArea : MonoBehaviour
{
    [SerializeField] private LevelUp levelUp;
    [SerializeField] private int levelUnlock;
    private void Start()
    {
        Unlock(levelUnlock);
    }
    public bool Unlock(int level)
    {
        if (levelUp.level >= level)
        {
            //unlock
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(false);
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER))
        {
            InventoryManager.Instance.txtPickUps.gameObject.SetActive(true);
            InventoryManager.Instance.txtPickUps.text = "Level " + levelUnlock + " to Unlock!";
            Unlock(levelUnlock);
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
