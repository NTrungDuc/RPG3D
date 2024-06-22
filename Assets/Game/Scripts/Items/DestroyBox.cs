using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBox : MonoBehaviour
{
    private float hpBox = 30;
    [SerializeField] private GameObject itemInBox;
    [SerializeField] private Animator animator;
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
        Destroy(gameObject);
    }
}
