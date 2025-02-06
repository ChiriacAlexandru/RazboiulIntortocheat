using UnityEngine;

public class EnemyMovementToPlayer : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float stoppingDistance = 1f;

    private Rigidbody rb;
    private Collider[] hitColliders;
    private int maxColliders = 20;
    private Transform target;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hitColliders = new Collider[maxColliders];
    }

    private void FixedUpdate()
    {
        target = GetClosestTarget();

        if (target != null)
        {
            MoveTowardsTarget(target.position);
        }
        else
        {
            rb.linearVelocity = Vector3.zero; // Oprește inamicul dacă nu are țintă
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

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Evită mișcarea pe Y

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            rb.MovePosition(rb.position + direction * movementSpeed * Time.fixedDeltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
