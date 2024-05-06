using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public enum Type { PlayerSword, EnemyAxe, PosionRed, PlayerSkill };
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
        if (type == Type.EnemyAxe)
        {
            if (other.CompareTag(Constant.TAG_PLAYER) && !hasAttacked)
            {
                other.GetComponent<PlayerMovement>().takeDamage(value);
                hasAttacked = true;
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
