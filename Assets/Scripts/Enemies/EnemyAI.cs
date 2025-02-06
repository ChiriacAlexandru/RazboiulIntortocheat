using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float stoppingDistance = 1f;

    [Header("Random Movement Settings")]
    [SerializeField] private float minX = -10f, maxX = 10f;
    [SerializeField] private float minZ = -10f, maxZ = 10f;
    [SerializeField] private float changeTargetCooldown = 2f; // Pauză între schimbări

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 10f;
    private Collider[] hitColliders;
    private int maxColliders = 20;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackDuration = 1f; // Durata animației de atac
    [SerializeField] private Collider swordCollider; // Referință la collider-ul sabiei

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Transform detectedTarget;
    private float nextChangeTime;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    private Animator animator; // Referință la Animator
    private EnemySword enemySword; // Referință la scriptul sabiei

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Obține componenta Animator
        hitColliders = new Collider[maxColliders];

        // Obține referința la sabie
        enemySword = GetComponentInChildren<EnemySword>();
        if (enemySword != null)
        {
            enemySword.SetActive(false); // Dezactivează sabia la început
        }

        if (swordCollider != null)
        {
            swordCollider.enabled = false; // Dezactivează collider-ul sabiei la început
        }

        GenerateRandomTarget();
    }

    private void FixedUpdate()
    {
        if (isAttacking) return; // Dacă inamicul atacă, nu face nimic altceva

        detectedTarget = GetClosestTarget();

        if (detectedTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, detectedTarget.position);

            if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
            {
                AttackTarget();
            }
            else if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget(detectedTarget.position, runSpeed, "IsRuning");
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPosition) <= stoppingDistance && Time.time >= nextChangeTime)
            {
                GenerateRandomTarget();
                nextChangeTime = Time.time + changeTargetCooldown;
            }

            MoveTowardsTarget(targetPosition, walkSpeed, "IsWalking");
        }
    }

    private Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, hitColliders);

        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].CompareTag("Player") || hitColliders[i].CompareTag("PlayerArmy"))
            {
                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = hitColliders[i].transform;
                }
            }
        }

        return closestTarget;
    }

    private void GenerateRandomTarget()
    {
        targetPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
    }

    private void MoveTowardsTarget(Vector3 targetPos, float speed, string animationState)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;

        if (Vector3.Distance(transform.position, targetPos) > stoppingDistance)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, Time.fixedDeltaTime * 5f);

            // Activează animația corespunzătoare
            animator.SetBool("IsWalking", animationState == "IsWalking");
            animator.SetBool("IsRuning", animationState == "IsRuning");
        }
        else
        {
            // Dezactivează toate animațiile dacă se oprește
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRuning", false);
        }
    }

    private void AttackTarget()
    {
        if (isAttacking) return;

        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        // Dezactivează celelalte animații
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRuning", false);

        // Pornește animația de atac folosind o variabilă booleană
        animator.SetBool("IsAttacking", true);

        // Activează sabia și collider-ul sabiei
        if (enemySword != null)
        {
            enemySword.SetActive(true);
        }
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }

        // Dezactivează sabia și collider-ul după durata atacului
        Invoke("EndAttack", attackDuration);
    }

    private void EndAttack()
    {
        isAttacking = false;

        // Oprește animația de atac
        animator.SetBool("IsAttacking", false);

        // Dezactivează sabia și collider-ul sabiei
        if (enemySword != null)
        {
            enemySword.SetActive(false);
        }
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }

        // Reactivează animațiile de mișcare dacă este necesar
        if (detectedTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, detectedTarget.position);
            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget(detectedTarget.position, runSpeed, "IsRuning");
            }
        }
        else
        {
            MoveTowardsTarget(targetPosition, walkSpeed, "IsWalking");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.5f);
    }
}