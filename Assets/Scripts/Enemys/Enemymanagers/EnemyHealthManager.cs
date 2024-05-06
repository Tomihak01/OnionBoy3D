using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    EnemyManager enemyManager;

    [Header("Health")]
    public int currentHealth = 100;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    public void DealDamage(int damage)
    {
        currentHealth = currentHealth - Mathf.RoundToInt(damage);
        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            enemyManager.isDead = true;
            enemyManager.enemyAnimatorManager.PlayTargetActionAnimation("CarrotDeath1");
        }
    }
}
