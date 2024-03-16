using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float rotationSpeed = 360f;
    public float moveIntervalMin = 5f;
    public float moveIntervalMax = 15f;
    public float chaseRange = 15f;
    public float stopChaseRange = 20f;

    protected float moveSpeed;
    protected bool isMoving = false;
    protected bool isPlayerDetected = false;
    private Transform player;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetEnemyStats();
        StartCoroutine(MoveAndRotate());
    }

    protected virtual void SetEnemyStats()
    {
        moveSpeed = 3f;
    }

    void Update()
    {
        DetectPlayer();
        MoveForward();
    }

    void MoveForward()
    {
        if (isMoving)
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator MoveAndRotate()
    {
        while (true)
        {
            float rotateTime = Random.Range(1f, 10f);
            float moveTime = Random.Range(5f, 10f);

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

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / rotateTime;
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            isMoving = true;
            yield return new WaitForSeconds(moveTime);
            isMoving = false;
            yield return null;
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
            }
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
        }
        else
        {
            if (!isMoving)
            {
                StartCoroutine(MoveAndRotate());
            }

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
        }
    }

    protected virtual void AttackPlayer()
    {
        Debug.Log("Hyökkäys pelaajaa vastaan!");
    }
}
