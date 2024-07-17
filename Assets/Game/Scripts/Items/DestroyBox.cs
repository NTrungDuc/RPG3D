using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DestroyBox : MonoBehaviour
{
    private float hpBox = 30;
    [SerializeField] private GameObject itemInBox;
    [SerializeField] private Items Item;
    [SerializeField] private Animator animator;
    private string uniqueID;
    private IEnumerator Start()
    {
        yield return null;

        uniqueID = Item.id + "_" + transform.position.ToString();
        if (GameManager.Instance.idItemsDestroyed.Contains(uniqueID))
        {
            Destroy(gameObject);
        }
    }
    public void takeDamage(float damage)
    {
        hpBox -= damage;
        if (hpBox <= 0)
        {
            StartCoroutine(waitCrashBox(20f));
        }
    }
    IEnumerator waitCrashBox(float time)
    {
        animator.SetBool(Constant.ANIM_BOX_CRASH, true);
        itemInBox.SetActive(true);
        yield return new WaitForSeconds(time);
        GameManager.Instance.idItemsDestroyed.Add(uniqueID);
        Destroy(gameObject);
    }
}
