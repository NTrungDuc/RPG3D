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
    float chaseRange = 10f;
    float jumpattackRange = 5f; //boss
    public void OnEnter(BotController bot)
    {
        bot.SetRandomTargetFollow();
        duration = Random.Range(1f, 4f);
    }
    public void OnExecute(BotController bot)
    {
        timer += Time.deltaTime;
        bot.FollowTarget(chaseRange);
        if (!bot.IsHaveTargetInRange(chaseRange) && timer > duration)
        {
            bot.ChangeState(new IdleState());
        }
        if (bot.enemyType == EnemyType.Normal || bot.enemyType == EnemyType.Boss)
        {
            if (bot.IsHaveTargetInRange(attackRange) && timer > 1f)
            {
                bot.ChangeState(new AttackState());
            }
        }
        if (bot.enemyType == EnemyType.Boss)
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
                bot.ChangeState(new AttackState());
            }
        }
    }
    public void OnExit(BotController bot)
    {

    }
}
