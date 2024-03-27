using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float rotationSpeed = 180f; // K‰‰ntymisnopeus astetta per sekunti
    public float moveIntervalMin = 5f;
    public float moveIntervalMax = 15f;
    public float chaseRange = 15f;
    public float stopChaseRange = 20f;

    protected float moveSpeed;
    protected bool isMoving = false;
    protected bool isPlayerDetected = false;
    private Transform player;
    private Vector3 lastPlayerPosition;

    protected float chaseMoveSpeed; // Muutettavissa oleva nopeus vihollisen seuratessa pelaajaa
    private float originalMoveSpeed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetEnemyStats();
        StartCoroutine(MoveAndRotate());
    }

    protected virtual void SetEnemyStats()
    {
        moveSpeed = 3f;
        chaseMoveSpeed = 5f;
        originalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        DetectPlayer();
    }

    IEnumerator MoveAndRotate()
    {
        while (true)
        {
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation;

            if (isPlayerDetected)
            {
                targetRotation = Quaternion.LookRotation(lastPlayerPosition - transform.position);
            }
            else
            {
                targetRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }

            float rotateTime = Random.Range(1f, 5f); // Satunnainen k‰‰ntymisen aika 1-5 sekuntia
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / rotateTime;
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            isMoving = true;

            float moveTime = Random.Range(5f, 10f);
            float elapsedTime = 0f;
            while (elapsedTime < moveTime)
            {
                elapsedTime += Time.deltaTime;
                if (isPlayerDetected)
                {
                    transform.position = Vector3.MoveTowards(transform.position, lastPlayerPosition, chaseMoveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                }
                yield return null;
            }

            isMoving = false;

            yield return new WaitForSeconds(Random.Range(moveIntervalMin, moveIntervalMax));
        }
    }

    void DetectPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                isPlayerDetected = true;
                lastPlayerPosition = hit.transform.position;
                Debug.Log("Pelaaja havaittu. Pelaajan sijainti: " + lastPlayerPosition);
            }
        }
        else
        {
            isPlayerDetected = false;
        }

        Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.green);

        if (isPlayerDetected)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, lastPlayerPosition);

        if (distanceToPlayer > chaseRange || distanceToPlayer > stopChaseRange)
        {
            isPlayerDetected = false;
            moveSpeed = originalMoveSpeed; // Palautetaan alkuper‰inen liikkumisnopeus
        }
        else
        {
            Debug.Log("Pelaaja havaittu, et‰isyys: " + distanceToPlayer);

            if (!isMoving)
            {
                StartCoroutine(MoveAndRotate());
            }

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                moveSpeed = chaseMoveSpeed; // Asetetaan nopeudeksi chaseMoveSpeed

                // P‰ivitet‰‰n vihollisen sijainti pelaajan suuntaan
                transform.position = Vector3.MoveTowards(transform.position, lastPlayerPosition, chaseMoveSpeed * Time.deltaTime);
            }
        }
    }

    protected virtual void AttackPlayer()
    {
        Debug.Log("Hyˆkk‰ys pelaajaa vastaan!");
        // Voit lis‰t‰ hyˆkk‰yslogiikkaa, viivett
    }
}