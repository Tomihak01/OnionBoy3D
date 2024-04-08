using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    public IdleState startingState;

    [Header("Current State")]
    [SerializeField] private State currentState;

    [Header("Current Target")]
    public PlayerManager currentTarget;
    public float distanceFromCurrentTarget;

    [Header("Animator")]
    public Animator animator;

    [Header("Navmesh Agent")]
    public NavMeshAgent enemyNavmeshAgent;


    [Header("RigidBody")]
    public Rigidbody enemyRigidbody;

    [Header("Locomotion")]
    public float rotationSpeed = 5;

    [Header("Attack")]
    public float mininumAttackDistance = 1;


    private void Awake()
    {
        currentState = startingState;
        enemyNavmeshAgent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        enemyNavmeshAgent.transform.localPosition = Vector3.zero;
        if(currentTarget != null)
        {
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
        HandelStateMachine();
    }

}
