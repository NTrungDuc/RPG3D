using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public enum Type { PlayerSword, EnemyAxe, PosionRed, PlayerSkill, Staves, EnemyKick, EnemyBoomb, EagleAttack, TotoiseAttack };
    public Type type;
    [SerializeField] public float value;
    private bool hasAttacked = false;
    public bool hasSkill = false;
    public void Use()
    {
        if (type == Type.PosionRed)
        {
            if (Input.GetMouseButtonUp(0))
            {
                PlayerMovement.Instance.UsePosion(value);
            }
        }
        if (type == Type.PlayerSword)
        {
            PlayerMovement.Instance.Attack();
        }
        if (type == Type.Staves)
        {
            PlayerMovement.Instance.useAbilities(Mathf.CeilToInt(value));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (type == Type.PlayerSword)
        {
            if (other.CompareTag(Constant.TAG_ENEMY) && !hasAttacked && PlayerMovement.Instance.state == PlayerState.Attack)
            {
                //Debug.Log("Player damaged");
                other.GetComponent<BotController>().takeDamage(value);
                hasAttacked = true;
            }
            if (other.CompareTag(Constant.TAG_BOX) && !hasAttacked && PlayerMovement.Instance.state == PlayerState.Attack)
            {
                //Debug.Log("Player damaged");
                other.GetComponent<DestroyBox>().takeDamage(value);
                hasAttacked = true;
            }
        }
        if (type == Type.EnemyAxe || type == Type.EnemyKick)
        {
            if (other.CompareTag(Constant.TAG_PLAYER))
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                other.GetComponent<PlayerMovement>().takeDamage(value);
            }
        }
        if (type == Type.EagleAttack)
        {
            if (other.CompareTag(Constant.TAG_PLAYER))
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                other.GetComponent<PlayerMovement>().takeDamage(value);
            }
            if (other.CompareTag(Constant.TAG_SHIELD))
            {
                StartCoroutine(BotController.Instance.Stun(7f));
            }
        }
        if (type == Type.EnemyBoomb)
        {
            if (other.CompareTag(Constant.TAG_PLAYER))
            {
                other.GetComponent<PlayerMovement>().takeDamage(value);
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                StartCoroutine(WaitAnimation(2f));                
            }
        }
        if(type == Type.PlayerSkill)
        {
            if (other.CompareTag(Constant.TAG_ENEMY))
            {
                Debug.Log("skill vfx");
                other.GetComponent<BotController>().takeDamage(value);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(type == Type.TotoiseAttack)
        {
            if (other.CompareTag(Constant.TAG_PLAYER))
            {
                other.GetComponent<PlayerMovement>().takeDamage(value * Time.deltaTime);
            }
        }
    }
    IEnumerator WaitAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        hasAttacked = false;
    }
}
