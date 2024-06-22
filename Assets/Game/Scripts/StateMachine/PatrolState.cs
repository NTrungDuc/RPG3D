using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float timer = 0;
    float duration;
    //range attack
    float attackRange = 2f;
    float attackRangeEagle = 5.2f;
    float attackRangeTotoise = 7f;
    float attackRangeAcitTotoise = 20f;
    float chaseRange = 10f;
    float jumpattackRange = 5f; //boss
    public void OnEnter(BotController bot)
    {
        bot.SetRandomTargetFollow();
        duration = Random.Range(1f, 4f);
        if (bot.enemyType == EnemyType.Boss_Tortoise)
        {
            chaseRange = 25f;
        }
    }
    public void OnExecute(BotController bot)
    {
        timer += Time.deltaTime;
        bot.FollowTarget(chaseRange);
        if (!bot.IsHaveTargetInRange(chaseRange) && timer > duration)
        {
            bot.ChangeState(new IdleState());
        }
        if (bot.enemyType == EnemyType.Normal || bot.enemyType == EnemyType.Boss_minotaur || bot.enemyType == EnemyType.Enemy_Boom)
        {
            if (bot.IsHaveTargetInRange(attackRange) && timer > 1f)
            {
                bot.ChangeState(new AttackState("CloseRange"));
            }
        }
        if (bot.enemyType == EnemyType.Boss_minotaur)
        {
            if (bot.IsHaveTargetInRange(jumpattackRange))
            {
                bot.BossJumpAttack();
            }
        }
        if (bot.enemyType == EnemyType.Eagle)
        {
            if (bot.IsHaveTargetInRange(attackRangeEagle) && timer > 1f)
            {
                bot.ChangeState(new AttackState("CloseRange"));
            }
        }
        if (bot.enemyType == EnemyType.Boss_Tortoise)
        {
            if (bot.IsHaveTargetInRange(attackRangeTotoise) && timer > 3f)
            {
                bot.ChangeState(new AttackState("CloseRange"));
            }
            else if (bot.IsHaveTargetInRange(attackRangeAcitTotoise) && timer > 3f)
            {
                bot.ChangeState(new AttackState("MediumRange"));
            }
        }
    }
    public void OnExit(BotController bot)
    {

    }
}
