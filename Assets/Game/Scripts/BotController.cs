using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class BotController : MonoBehaviour
{
    //information
    public EnemyType enemyType;
    [SerializeField] private int id;
    [SerializeField] private float enemyMaxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private int experienceValue = 30;
    [SerializeField] private float coinDropped = 30;
    [SerializeField] private LevelUp playerExp;
    float initialSpeed;
    //col weapons
    [SerializeField] private Collider attack_1;
    [SerializeField] private Collider attack_2;
    [SerializeField] private Collider bodyCol;
    //patrol
    private NavMeshAgent agent;
    [SerializeField] private float range;
    Vector3 patrolPoint;
    //target,attack
    [SerializeField] Transform Target;
    //component
    [SerializeField] private Transform centrePoint;
    public bool isJumpAttack = true;
    private Animator animator;
    bool isDeath = false;
    private IState currentState;
    public IState CurrentState { get => currentState; set => currentState = value; }

    private static BotController instance;
    public static BotController Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath)
        {
            return;
        }
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }
    public void OnInit()
    {
        currentHealth = enemyMaxHealth;
        initialSpeed = agent.speed;
        ChangeState(new IdleState());
    }
    public void SetRandomTargetFollow()
    {
        Run();
        patrolPoint = GetRandomPointOnNavMesh(centrePoint.position, range);
        agent.SetDestination(patrolPoint);
    }
    Vector3 GetRandomPointOnNavMesh(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0f;
        randomDirection.Normalize();

        Vector3 randomPoint = origin + randomDirection * Random.Range(0f, distance);

        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomPoint, out navMeshHit, distance, NavMesh.AllAreas);

        return navMeshHit.position;
    }
    public void FollowTarget(float range)
    {
        if (IsHaveTargetInRange(range))
        {
            Run();
            agent.SetDestination(Target.position);
        }
    }
    public void Run()
    {
        ChangeAnim(Constant.ANIM_RUN, true);
        ExitAttack();
        agent.isStopped = false;
    }
    public bool IsHaveTargetInRange(float range)
    {
        float distance = Vector3.Distance(Target.position, transform.position);
        //Debug.Log(distance);
        if (distance < range)
        {
            return true;
        }
        return false;
    }
    public bool checkDestination()
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }
    public void CloseRangeAttack()
    {
        transform.LookAt(Target);
        if (enemyType == EnemyType.Normal || enemyType == EnemyType.Eagle || enemyType == EnemyType.Enemy_Boom)
        {
            attack_1.enabled = true;
            ChangeAnim(Constant.ANIM_ATTACK, true);
        }
        if (enemyType == EnemyType.Boss_minotaur)
        {
            int attackType = Random.Range(0, 4);
            BossMinotaurAttack(attackType);
        }
        if (enemyType == EnemyType.Boss_Tortoise)
        {
            int attackType = Random.Range(0, 2);
            BossTotoiseAttack(attackType);
        }
    }
    public void MediumRangeAttack()
    {
        transform.LookAt(Target);
        if (enemyType == EnemyType.Boss_Tortoise)
        {
            BossTotoiseAttack(2);
        }
    }
    public void BossMinotaurAttack(int abilities)
    {
        switch (abilities)
        {
            case 0:
                attack_1.enabled = true;
                ChangeAnim(Constant.ANIM_ABILITIES_1, true);
                break;
            case 1:
                attack_1.enabled = true;
                ChangeAnim(Constant.ANIM_ABILITIES_2, true);
                break;
            case 2:
                attack_1.enabled = true;
                ChangeAnim(Constant.ANIM_ABILITIES_3, true);
                break;
            case 3:
                attack_2.enabled = true;
                ChangeAnim(Constant.ANIM_ABILITIES_4, true);
                break;
        }
    }
    public void BossTotoiseAttack(int abilities)
    {
        switch (abilities)
        {
            case 0:
                StartCoroutine(WaitTotoiseAttack_1());
                SoundManager.Instance.attackTotoise_1.Play();
                ChangeAnim(Constant.ANIM_ABILITIES_1, true);
                break;
            case 1:
                SoundManager.Instance.attackTotoise_3.Play();
                ChangeAnim(Constant.ANIM_ABILITIES_2, true);
                break;
            case 2:
                StartCoroutine(SoundManager.Instance.TotoiseAttackAcis());
                ChangeAnim(Constant.ANIM_ABILITIES_3, true);
                break;
        }
    }
    public IEnumerator WaitTotoiseAttack_1()
    {
        attack_1.enabled = true;
        yield return new WaitForSeconds(3f);
        attack_1.enabled = false;
    }
    public void ExitAttack()
    {
        if (enemyType == EnemyType.Normal || enemyType == EnemyType.Eagle)
        {
            ChangeAnim(Constant.ANIM_ATTACK, false);
        }
        if (enemyType == EnemyType.Boss_minotaur)
        {
            ChangeAnim(Constant.ANIM_ABILITIES_1, false);
            ChangeAnim(Constant.ANIM_ABILITIES_2, false);
            ChangeAnim(Constant.ANIM_ABILITIES_3, false);
            ChangeAnim(Constant.ANIM_ABILITIES_4, false);
            ChangeAnim(Constant.ANIM_JUMP_ATTACK, false);
        }
        if (enemyType == EnemyType.Boss_Tortoise)
        {
            ChangeAnim(Constant.ANIM_ABILITIES_1, false);
            ChangeAnim(Constant.ANIM_ABILITIES_2, false);
            ChangeAnim(Constant.ANIM_ABILITIES_3, false);
        }
    }
    public void BossJumpAttack()
    {
        if (isJumpAttack)
        {
            agent.speed = initialSpeed * 2;
            ChangeAnim(Constant.ANIM_JUMP_ATTACK, true);
            StartCoroutine(WaitForAnimationJumpAttackToEnd());
        }
    }
    private IEnumerator WaitForAnimationJumpAttackToEnd()
    {
        yield return new WaitForSeconds(3f);
        isJumpAttack = false;
        agent.speed = initialSpeed;
    }

    public IEnumerator Stun(float timeStun)
    {
        bodyCol.enabled = true;
        ChangeAnim(Constant.ANIM_DIZZY, true);

        yield return new WaitForSeconds(timeStun);
        bodyCol.enabled = false;
        ChangeAnim(Constant.ANIM_DIZZY, false);
    }
    public void stopMoving()
    {
        agent.isStopped = true;
    }
    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }
    public void ChangeAnim(string animName, bool isChange)
    {
        animator.SetBool(animName, isChange);
    }
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //die
            StartCoroutine(OnDeath());
        }
    }
    public IEnumerator OnDeath()
    {
        isDeath = true;
        //ChangeAnim(Constant.ANIM_DIE, true);
        animator.SetTrigger(Constant.ANIM_DIE);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        playerExp.GainXP(experienceValue, coinDropped);
    }
}
public enum EnemyType
{
    Normal,
    Eagle,
    Boss_minotaur,
    Boss_Tortoise,
    Enemy_Boom
}
