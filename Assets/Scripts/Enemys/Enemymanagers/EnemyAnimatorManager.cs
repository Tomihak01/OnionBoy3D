using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : MonoBehaviour
{
    EnemyManager enemyManager;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    public void PlayGrappleAnimation(string grappleanimation, bool useRootMotion)
    {
        enemyManager.animator.applyRootMotion = true;
        enemyManager.isPerformingAction = true;
        enemyManager.animator.CrossFade(grappleanimation, 0.2f);
    }

    public void PlayTargetAttackAnimation(string attackAnimation)
    {
        enemyManager.animator.applyRootMotion = true;
        enemyManager.isPerformingAction = true;
        enemyManager.animator.CrossFade(attackAnimation, 0.2f);
    }

    public void PlayTargetActionAnimation(string actionAnimation)
    {
       enemyManager.animator.applyRootMotion = true;
        enemyManager.isPerformingAction = true;
        enemyManager.animator.CrossFade(actionAnimation, 0.2f);
    }

}
