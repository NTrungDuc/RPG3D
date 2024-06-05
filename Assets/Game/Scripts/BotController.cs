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
    [SerializeField] private LevelUp playerExp;
    float initialSpeed;
    //col weapons
    [SerializeField] private Collider axeCol;
    [SerializeField] private Collider kickCol;
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
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
        checkDestination();
    }
    public void OnInit()
    {
        currentHealth = enemyMaxHealth;
        initialSpeed = agent.speed;
        ChangeState(new IdleState());
    }
    public void checkDestination()
    {
        if (Vector3.Distance(transform.position, patrolPoint) < 2.8f)
        {
            ChangeAnim(Constant.ANIM_RUN, false);
        }
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
    public void Attack()
    {
        transform.LookAt(Target);
        if (enemyType == EnemyType.Normal || enemyType == EnemyType.Eagle)
        {
            axeCol.enabled = true;
            ChangeAnim(Constant.ANIM_ATTACK, true);
        }
        if (enemyType == EnemyType.Boss)
        {
            int attackType = Random.Range(0, 4);
            BossAttack(attackType);
        }
    }
    public void BossAttack(int abilities)
    {
        switch (abilities)
        {
            case 0:
                axeCol.enabled = true;
                ChangeAnim(Constant.ANIM_ABILITIES_1, true);
                break;
            case 1:
                axeCol.enabled = true;
                ChangeAnim(Constant.ANIM_ABILITIES_2, true);
                break;
            case 2:
                axeCol.enabled = true;
                ChangeAnim(Constant.ANIM_ABILITIES_3, true);
                break;
            case 3:
                kickCol.enabled = true;
                ChangeAnim(Constant.ANIM_ABILITIES_4, true);
                break;
        }
    }
    public void ExitAttack()
    {
        if (enemyType == EnemyType.Normal || enemyType == EnemyType.Eagle)
        {
            ChangeAnim(Constant.ANIM_ATTACK, false);
        }
        if (enemyType == EnemyType.Boss)
        {
            ChangeAnim(Constant.ANIM_ABILITIES_1, false);
            ChangeAnim(Constant.ANIM_ABILITIES_2, false);
            ChangeAnim(Constant.ANIM_ABILITIES_3, false);
            ChangeAnim(Constant.ANIM_ABILITIES_4, false);
            ChangeAnim(Constant.ANIM_JUMP_ATTACK, false);
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
        stopMoving();
        ChangeAnim(Constant.ANIM_DIE, true);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        playerExp.GainXP(experienceValue);
    }
}
public enum EnemyType
{
    Normal,
    Eagle,
    Boss
}
