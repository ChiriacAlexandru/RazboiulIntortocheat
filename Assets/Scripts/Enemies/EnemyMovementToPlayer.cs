using UnityEngine;

public class EnemyMovementToPlayer : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f; // Raza de detectare
    [SerializeField] private float movementSpeed = 3f; // Viteza de mișcare
    [SerializeField] private float stoppingDistance = 1f; // Distanța la care inamicul se oprește

    private Rigidbody rb; // Referință la Rigidbody
    private Collider[] hitColliders; // Array prealocat pentru depistarea coliziunilor
    private int maxColliders = 20; // Numărul maxim de coliziuni de depistat

    private void Start()
    {
        // Obține referința la componentul Rigidbody
        rb = GetComponent<Rigidbody>();

        // Inițializează array-ul pentru depistarea coliziunilor
        hitColliders = new Collider[maxColliders];
    }

    void Update()
    {
        // Găsește cel mai apropiat obiect (player sau soldat din armată)
        GameObject closestTarget = GetClosestTarget();

        if (closestTarget != null)
        {
            // Mișcă inamicul către cel mai apropiat obiect
            MoveTowardsTarget(closestTarget.transform.position);
        }
        else
        {
            // Dacă nu există o țintă, oprește mișcarea
            rb.linearVelocity = Vector3.zero;
        }
    }

    // Găsește cel mai apropiat obiect cu tag-ul "Player" sau "PlayerArmy"
    private GameObject GetClosestTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        // Folosește OverlapSphereNonAlloc pentru a evita alocări de memorie
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, hitColliders);

        for (int i = 0; i < numColliders; i++)
        {
            // Verifică dacă obiectul are tag-ul "Player" sau "PlayerArmy"
            if (hitColliders[i].CompareTag("Player") || hitColliders[i].CompareTag("PlayerArmy"))
            {
                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = hitColliders[i].gameObject;
                }
            }
        }

        return closestTarget;
    }

    // Mișcă inamicul către o poziție țintă
    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        // Calculează direcția de mișcare
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Ignoră axa Y pentru mișcare doar pe planul XZ (opțional)
        direction.y = 0;

        // Verifică dacă inamicul este în afara distanței de oprire
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            // Aplică viteza de mișcare
            rb.linearVelocity = direction * movementSpeed;
        }
        else
        {
            // Oprește mișcarea dacă inamicul este suficient de aproape
            rb.linearVelocity = Vector3.zero;
        }
    }

    // Opțional: vizualizare a razei de detectare în editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}