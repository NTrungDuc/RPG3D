using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    float timer = 0;
    float timeDelay = 1f;
    private string attackType;

    public AttackState(string attackType)
    {
        this.attackType = attackType;
    }
    public void OnEnter(BotController bot)
    {
        timer = 0;
        bot.stopMoving();

        switch (attackType)
        {
            case "CloseRange":
                bot.CloseRangeAttack();
                break;
            case "MediumRange":
                timeDelay = 2f;
                bot.MediumRangeAttack();
                break;
        }
    }
    public void OnExecute(BotController bot)
    {
        if(bot.enemyType == EnemyType.Enemy_Boom)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer > timeDelay)
        {
            bot.ChangeState(new PatrolState());
        }
    }
    public void OnExit(BotController bot)
    {

    }
}
