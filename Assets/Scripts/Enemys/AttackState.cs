using UnityEngine;


public class AttackState : State
{
    public override State Tick(EnemyManager enemyManager)
    {
        Debug.Log("Attack");
        enemyManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
        return this;
    }
}
