using System.Collections.Generic;
using UnityEngine;


public class AttackState : State
{

    PursueTargetState pursueTargetState;

    [Header("Enemy Attacks")]
    public EnemyAttackAction[] enemyAttackActions;

    [Header("Potential Attacks Performable Right Now")]
    public List<EnemyAttackAction> potentialAttacks;

    [Header("Current Attacks being Performe")]
    public EnemyAttackAction currentAttack;

    [Header("State Flags")]
    public bool hasPerformedAttack;

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(EnemyManager enemyManager)
    {
        enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);

        if (enemyManager.isPerformingAction)
        {
            enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }
        if (!hasPerformedAttack && enemyManager.attackCoolDownTimer <= 0)
        {
            if (currentAttack == null)
            {
                GetNewAttack(enemyManager);
            }
            else
            {
                AttackTarget(enemyManager);
            }
        }

        if (hasPerformedAttack)
        {
            ResetStateFlags();
            return pursueTargetState;
        }
        else
        {
            return this;
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
            currentAttack = potentialAttacks[randomValue];
            potentialAttacks.Clear();
        }
    }

    private void AttackTarget(EnemyManager enemyManager)

    {
        if (currentAttack != null)
        {
            hasPerformedAttack = true;
            enemyManager.attackCoolDownTimer = currentAttack.attackCooldown;
            enemyManager.enemyAnimatorManager.PlayTargetAttackAnimation(currentAttack.attackAnimation);
        }
        else
        {
            Debug.LogWarning("enemy is attemping to perform attack but has no attack");
        }
    }

    private void ResetStateFlags()
    {
        hasPerformedAttack = false;
    }

}
