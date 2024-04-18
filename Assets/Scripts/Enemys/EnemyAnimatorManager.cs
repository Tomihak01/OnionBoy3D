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
    public void PlayTargetAttackAnimation(string attackAnimation)
    {
        enemyManager.animator.applyRootMotion = true;
        enemyManager.isPerformingAction = true;
        enemyManager.animator.CrossFade(attackAnimation, 0.2f);
    }

}
