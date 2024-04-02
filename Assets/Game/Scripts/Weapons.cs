using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum Type { PlayerSword, EnemyAxe };
    public Type type;
    [SerializeField] BotController botControler;
    [SerializeField] PlayerMovement playerMovement;

    private float damage;
    private void Start()
    {
        if (type == Type.PlayerSword)
        {
            damage = playerMovement.damagePlayer;
        }
        else if(type == Type.EnemyAxe)
        {
            damage = botControler.damageEnemy;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (type == Type.PlayerSword)
        {
            if (other.CompareTag(Constant.TAG_ENEMY) && PlayerMovement.Instance.state == PlayerState.Attack)
            {
                other.GetComponent<BotController>().takeDamage(damage);
            }
        }
        else
        {
            //player take damage
        }
    }
}
