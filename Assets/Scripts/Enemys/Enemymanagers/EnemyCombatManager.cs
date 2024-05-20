using UnityEngine;

public class EnemyCombatManager : MonoBehaviour
{
    [Header("Attack Damage")]
    public int attackDamage = 50;

    private EnemyManager enemyManager;
    [SerializeField] private EnemyGrappleCollider rightGrappleCollider;
    [SerializeField] private EnemyGrappleCollider leftGrappleCollider;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        LoadGrappleColliders();
    }

    public void SetAttackType(EnemyAttackType attackType)
    {
        rightGrappleCollider.attackType = attackType;
        leftGrappleCollider.attackType = attackType;
    }

    private void LoadGrappleColliders()
    {
        EnemyGrappleCollider[] grappleColliders = GetComponentsInChildren<EnemyGrappleCollider>();

        foreach (var grappleCollider in grappleColliders)
        {
            if (grappleCollider.isRightGrappleCollider)
            {
                rightGrappleCollider = grappleCollider;
            }
            else
            {
                leftGrappleCollider = grappleCollider;
            }
        }
    }

    public void OpenGrappleColliders()
    {
        rightGrappleCollider.grappleCollider.enabled = true;
        leftGrappleCollider.grappleCollider.enabled = true;
    }

    public void CloseGrappleColliders()
    {
        rightGrappleCollider.grappleCollider.enabled = false;
        leftGrappleCollider.grappleCollider.enabled = false;
    }

    public void EnableRotationDuringAttack()
    {
        enemyManager.canRotate = true;
    }

    public void DisableRotationDuringAttack()
    {
        enemyManager.canRotate = false;
    }
}
