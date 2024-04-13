using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class BotController : MonoBehaviour
{
    //infor 
    [SerializeField] private int id;
    private float enemyMaxHealth = 100;
    [SerializeField] private float currentHealth;
    //patrol
    private NavMeshAgent agent;
    [SerializeField] private float range;
    Vector3 patrolPoint;
    //target,attack
    [SerializeField] Transform Target;
    //component
    [SerializeField] private Transform centrePoint;
    private Animator animator;
    private IState currentState;
    public IState CurrentState { get => currentState; set => currentState = value; }
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
        ChangeState(new IdleState());
    }
    public void checkDestination()
    {
        if(Vector3.Distance(transform.position, patrolPoint) < 2.8f)
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
        ChangeAnim(Constant.ANIM_ATTACK, false);
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
        ChangeAnim(Constant.ANIM_ATTACK, true);
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
    }
}
