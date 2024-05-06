using Unity.Mathematics;
using UnityEngine;

public class EnemyGrappleCollider : MonoBehaviour
{
    EnemyManager enemy;

    [Header("collider")]
    public Collider grappleCollider;

    [Header("Damage Hand collider")]
    public bool isRightGrappleCollider;

    [Header("Player Health")]
    public Health playerHealth;

    [Header("Current Attack Type")]
    public EnemyAttackType attackType;

    [Header("Damage")]
    public int damageAmount = 20;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyManager>();
        grappleCollider = GetComponent<Collider>();

        playerHealth = enemy.GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Health player = other.GetComponent<Health>();
             

            if (player != null)
            {

                DecideAttackAction(player);

            }
        }
    }
    private void DecideAttackAction(Health player)
    {
        if (attackType == EnemyAttackType.grapple)
        {
            enemy.enemyAnimatorManager.PlayGrappleAnimation("Grapple", true);
            enemy.animator.SetFloat("Vertical", 0);

            Quaternion targetEnemyRotation = Quaternion.LookRotation(player.transform.position - enemy.transform.position);
            enemy.transform.rotation = targetEnemyRotation;

            Quaternion targetPlayerRotation = Quaternion.LookRotation(enemy.transform.position - player.transform.position);
            player.transform.rotation = targetPlayerRotation;

            playerHealth.Damage(enemy.enemyCombatManager.grappleDamage);
        }
        else if (attackType == EnemyAttackType.punch)
        {
             playerHealth.Damage(enemy.enemyCombatManager.grappleDamage);
        }
        else if (attackType == EnemyAttackType.swipe)
        {
            playerHealth.Damage(enemy.enemyCombatManager.grappleDamage);
        }
        else if (attackType == EnemyAttackType.dropkick)
        {
            playerHealth.Damage(enemy.enemyCombatManager.grappleDamage);
        }

    }
}
