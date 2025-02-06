using UnityEngine;

public class EnemyRandomMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float minX = -10f, maxX = 10f;
    [SerializeField] private float minZ = -10f, maxZ = 10f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float changeTargetCooldown = 1f;

    private Vector3 targetPosition;
    private Rigidbody rb;
    private float nextChangeTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GenerateRandomTarget();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= stoppingDistance && Time.time >= nextChangeTime)
        {
            GenerateRandomTarget();
            nextChangeTime = Time.time + changeTargetCooldown;
        }

        MoveTowardsTarget();
    }

    private void GenerateRandomTarget()
    {
        targetPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Evităm mișcarea pe Y

        rb.MovePosition(rb.position + direction * movementSpeed * Time.fixedDeltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.5f);
    }
}
