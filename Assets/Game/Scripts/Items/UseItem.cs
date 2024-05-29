using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public enum Type { PlayerSword, EnemyAxe, PosionRed, PlayerSkill, Staves_0, Staves_1, Staves_2, EnemyKick };
    public Type type;

    [SerializeField] private float value;
    private bool hasAttacked = false;
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
        if (type == Type.Staves_0)
        {
            PlayerMovement.Instance.useAbilities(0);
        }
        if (type == Type.Staves_1)
        {
            PlayerMovement.Instance.useAbilities(1);
        }
        if (type == Type.Staves_2)
        {
            PlayerMovement.Instance.useAbilities(2);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (type == Type.PlayerSword)
        {
            if (other.CompareTag(Constant.TAG_ENEMY) && !hasAttacked && PlayerMovement.Instance.state == PlayerState.Attack)
            {
                other.GetComponent<BotController>().takeDamage(value);
                hasAttacked = true;
            }
        }
        if (type == Type.EnemyAxe || type == Type.EnemyKick)
        {
            if (other.CompareTag(Constant.TAG_PLAYER))
            {
                //Debug.Log("take damage");
                gameObject.GetComponent<BoxCollider>().enabled = false;
                other.GetComponent<PlayerMovement>().takeDamage(value);
            }
        }
        if(type == Type.PlayerSkill)
        {
            if (other.CompareTag(Constant.TAG_ENEMY))
            {
                Debug.Log("skill vfx");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        hasAttacked = false;
    }
}
