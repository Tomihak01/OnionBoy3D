using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Actions/Enemy Attack Action")]
public class EnemyAttackAction : ScriptableObject
{

    [Header("AttackType")]
    public EnemyAttackType attackType;

    [Header("Attack Animation")]
    public string attackAnimation;

    [Header("Attack Cooldown")]
    public float attackCooldown = 5f;

    [Header("Attack Angels & Distances")]
    public float maximumAttackAngle = 20f;
    public float minimumAttackAngle = -20f;
    public float maximumAttackDistance = 3f;
    public float minimumAttackDistance = 1f;
    



}
