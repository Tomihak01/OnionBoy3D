using UnityEngine;


public class AttackState : State
{

    PursueTargetState pursueTargetState;

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

       
        
        if (enemyManager.distanceFromCurrentTarget > enemyManager.maximumAttackDistance)
        {
            return pursueTargetState;
        }

        if (enemyManager.isPerformingAction)
        {
            
            enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }
        if (!hasPerformedAttack && enemyManager.attackCoolDownTimer <= 0)
        {
          AttackTarget(enemyManager);
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

    private void AttackTarget(EnemyManager enemyManager)
    {
        if (enemyManager != null && currentAttack != null)
        {
            if (enemyManager.enemyCombatManager != null && enemyManager.enemyAnimatorManager != null)
            {
                hasPerformedAttack = true;
                enemyManager.enemyCombatManager.SetAttackType(currentAttack.attackType);
                enemyManager.attackCoolDownTimer = currentAttack.attackCooldown;
                enemyManager.enemyAnimatorManager.PlayTargetAttackAnimation(currentAttack.attackAnimation);
            }
            else
            {
                Debug.LogError("Either enemyCombatManager or enemyAnimatorManager is null");
            }
        }
        else
        {
            Debug.LogWarning("enemy is attempting to perform attack but has no attack or enemyManager is null");
        }
    }

    private void ResetStateFlags()
    {
        hasPerformedAttack = false;
        currentAttack = null;
    }

}
