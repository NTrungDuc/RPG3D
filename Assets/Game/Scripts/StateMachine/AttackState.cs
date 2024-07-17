using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    float timer = 0;
    float timeDelay = 2f;
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
                timeDelay = 1f;
                bot.CloseRangeAttack();
                break;
            case "MediumRange":                
                bot.MediumRangeAttack();
                break;
            case "BossCloseRange":
                bot.CloseRangeAttack();
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
