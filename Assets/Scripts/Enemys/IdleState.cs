using UnityEngine;

public class IdleState : State
{

    PursueTargetState pursueTargetState;

    [Header("Detection Layer")]
    [SerializeField] LayerMask detectionLayer;

    [Header("Linen Of Sight Detection")]
   [SerializeField]float characterEyeLevel = 1.5f;
    [SerializeField] LayerMask ignoreForLinenOfSightDetection;

    [Header("Detection Radius")]
    [SerializeField] float detectionRadius = 5f;


    [Header("Detection Angel Radius")]
    [SerializeField] float mininumDetectionRadiusAngle = -100;
    [SerializeField] float maxnumDetectionRadiusAngle = 180f;


    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(EnemyManager enemyManager)
    {
        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            FindTargetViaLineOfSight(enemyManager);
            return this;
        }

    }

    private void FindTargetViaLineOfSight(EnemyManager enemyManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            if (player != null)
            {
                

                Vector3 targetDirection = transform.position - player.transform.position;
                float viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);

                
                if (viewAbleAngle > mininumDetectionRadiusAngle && viewAbleAngle < maxnumDetectionRadiusAngle)
                {
                    RaycastHit hit;
                    
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterEyeLevel, player.transform.position.z);
                    Vector3 enemyStartPoint = new Vector3(transform.position.x, characterEyeLevel, transform.position.z);

                    Debug.DrawLine(playerStartPoint, enemyStartPoint, Color.red);

                    if (Physics.Linecast(playerStartPoint, enemyStartPoint, out hit, ignoreForLinenOfSightDetection))
                    {
                       
                    }
                    else
                    {
                        enemyManager.currentTarget = player;
                    }
                    enemyManager.currentTarget = player;
                }
            }
        }


    }

}
