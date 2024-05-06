using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    AttackState attackState;

    [Header("Enemy Attacks")]
    public EnemyAttackAction[] enemyAttackActions;

    [Header("Potential Attacks Performable Right Now")]
    public List<EnemyAttackAction> potentialAttacks;

    private void Awake()
    {
        attackState = GetComponent<AttackState>();
    }

    public override State Tick(EnemyManager enemyManager)
    {

        MoveTowardsCurrentTarget(enemyManager);
        RotateTowardsTarget(enemyManager);

        if(enemyManager.isPerformingAction)
        {

            RotateTowardsTargetWhilstAttacking(enemyManager);
            enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        if (enemyManager.distanceFromCurrentTarget <= enemyManager.maximumAttackDistance)
        {
            enemyManager.enemyNavmeshAgent.enabled = false;
            if (attackState.currentAttack == null)
            {
                GetNewAttack(enemyManager);
                return this;
            }
            else
            {
                enemyManager.enemyNavmeshAgent.enabled = false;
                return attackState;
            }

        }
        else
        {
            return this;
        }


    }
    private void MoveTowardsCurrentTarget(EnemyManager enemyManager)
    {
        enemyManager.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
    }


    private void RotateTowardsTarget(EnemyManager enemyManager)
    {
        enemyManager.enemyNavmeshAgent.enabled = true;
        enemyManager.enemyNavmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.enemyNavmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);

    }

    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
    {
        if (enemyManager.canRotate)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();
            if(direction == Vector3.zero)
            {
                direction = enemyManager.transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed * Time.deltaTime);
        }
    }

    private void GetNewAttack(EnemyManager enemyManager)
    {
        for (int i = 0; i < enemyAttackActions.Length; i++)
        {
            EnemyAttackAction enemyAttack = enemyAttackActions[i];

            if (enemyManager.distanceFromCurrentTarget <= enemyAttack.maximumAttackDistance && enemyManager.distanceFromCurrentTarget >= enemyAttack.minimumAttackDistance)
            {
                if (enemyManager.viewableAngleFromCurretTarget <= enemyAttack.maximumAttackDistance && enemyManager.viewableAngleFromCurretTarget >= enemyAttack.minimumAttackAngle)
                {
                    potentialAttacks.Add(enemyAttack);
                }

            }

        }

        int randomValue = Random.Range(0, potentialAttacks.Count);

        if (potentialAttacks.Count > 0)
        {
            attackState.currentAttack = potentialAttacks[randomValue];
            potentialAttacks.Clear();
        }
    }

}
