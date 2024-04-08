using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    AttackState attackState;


    private void Awake()
    {
        attackState = GetComponent<AttackState>();
    }

    public override State Tick(EnemyManager enemyManager)
    {

        MoveTowardsCurrentTarget(enemyManager);
        RotateTowardsTarget(enemyManager);

        if (enemyManager.distanceFromCurrentTarget<= enemyManager.mininumAttackDistance)
        {
            return attackState;
        }
        else
        {
            return this;
        }

        
    }
    private void MoveTowardsCurrentTarget(EnemyManager enemyManager)
    {
        enemyManager.animator.SetFloat("Vertical",1, 0.2f, Time.deltaTime);
    }


    private void RotateTowardsTarget(EnemyManager enemyManager)
    {
        enemyManager.enemyNavmeshAgent.enabled = true;
        enemyManager.enemyNavmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.enemyNavmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
    }


}
