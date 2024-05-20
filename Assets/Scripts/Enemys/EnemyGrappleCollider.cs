using UnityEngine;

public class EnemyGrappleCollider : MonoBehaviour
{
    private EnemyManager enemy;

    [Header("Collider")]
    public Collider grappleCollider;

    [Header("Damage Hand Collider")]
    public bool isRightGrappleCollider;

    [Header("Player Health")]
    public Health playerHealth;

    [Header("Current Attack Type")]
    public EnemyAttackType attackType;

    [Header("Damage")]
    public int damageAmount = 20;

    private void Awake()
    {
        // Get the EnemyManager component from the parent object
        enemy = GetComponentInParent<EnemyManager>();
        // Get the Collider component attached to this object
        grappleCollider = GetComponent<Collider>();

        // Get the Health component from the enemy (assuming the enemy has a Health component)
        playerHealth = enemy.GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
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
        // Perform different actions based on the attack type
        switch (attackType)
        {
            case EnemyAttackType.grapple:
                PerformGrapple(player);
                break;
            case EnemyAttackType.punch:
            case EnemyAttackType.swipe:
            case EnemyAttackType.dropkick:
                player.Damage(enemy.enemyCombatManager.attackDamage);
                break;
        }
    }

    private void PerformGrapple(Health player)
    {
        // Play the grapple animation
        enemy.enemyAnimatorManager.PlayGrappleAnimation("Grapple", true);
        enemy.animator.SetFloat("Vertical", 0);

        // Rotate enemy to face the player
        Quaternion targetEnemyRotation = Quaternion.LookRotation(player.transform.position - enemy.transform.position);
        enemy.transform.rotation = targetEnemyRotation;

        // Rotate player to face the enemy
        Quaternion targetPlayerRotation = Quaternion.LookRotation(enemy.transform.position - player.transform.position);
        player.transform.rotation = targetPlayerRotation;

        // Inflict damage to the player
        player.Damage(enemy.enemyCombatManager.attackDamage);
    }
}