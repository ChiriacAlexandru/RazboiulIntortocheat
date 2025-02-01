using UnityEngine;

public class EnemyRotationToPlayer : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f; // Raza de detectare
    [SerializeField] private float rotationSpeed = 5f; // Viteza de rotație

    private Collider[] hitColliders; // Array prealocat pentru depistarea coliziunilor
    private int maxColliders = 20; // Numărul maxim de coliziuni de depistat

    private void Start()
    {
        // Inițializează array-ul pentru depistarea coliziunilor
        hitColliders = new Collider[maxColliders];
    }

    void Update()
    {
        // Găsește cel mai apropiat soldat sau player
        GameObject closestTarget = GetClosestTarget();

        if (closestTarget != null)
        {
            // Calculează direcția către cel mai apropiat obiect
            Vector3 direction = (closestTarget.transform.position - transform.position).normalized;

            // Ignoră axa Y pentru rotație doar pe planul XZ (opțional)
            direction.y = 0;

            // Calculează rotația necesară
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Aplică rotația graduală
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime * 100f
            );
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

    // Opțional: vizualizare a razei de detectare în editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}