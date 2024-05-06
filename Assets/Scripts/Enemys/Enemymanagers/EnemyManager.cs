using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public EnemyAnimatorManager enemyAnimatorManager;
    public IdleState startingState;
    public EnemyHealthManager enemyHealthManager;
    public EnemyCombatManager enemyCombatManager;

    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;
    public Vector3 targetsDirection;
    public float distanceFromCurrentTarget;
    public float viewableAngleFromCurretTarget;

    [Header("Animator")]
    public Animator animator;

    [Header("Navmesh Agent")]
    public NavMeshAgent enemyNavmeshAgent;


    [Header("RigidBody")]
    public Rigidbody enemyRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5;

    [Header("Attack")]
    public float attackCoolDownTimer;
    public float minimumAttackDistance = 1;
    public float maximumAttackDistance = 3;

    [Header("Flags")]
    public bool isPerformingAction;
    public bool isDead;
    public bool canRotate;

    private void Awake()
    {
        currentState = startingState;
        enemyNavmeshAgent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyHealthManager = GetComponent<EnemyHealthManager>();
    }

    private void Update()
    {
        enemyNavmeshAgent.transform.localPosition = Vector3.zero;

        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer = attackCoolDownTimer - Time.deltaTime;
        }

        if (currentTarget != null)
        {
            targetsDirection = currentTarget.transform.position - transform.position;
            viewableAngleFromCurretTarget = Vector3.SignedAngle(targetsDirection, transform.forward, Vector3.up);
            distanceFromCurrentTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
    }

    private void HandelStateMachine()
    {
        State nextState;
        if (currentState != null)
        {
            nextState = currentState.Tick(this);
            if (nextState != null)
            {
                currentState = nextState;
            }

        }

    }



    private void FixedUpdate()
    {
        if (!isDead)
        {
            HandelStateMachine();
        }


    }

}
