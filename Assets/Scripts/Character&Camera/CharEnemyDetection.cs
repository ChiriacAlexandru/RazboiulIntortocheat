using UnityEngine;

public class PlayerFaceClosestEnemy : MonoBehaviour
{
    private string enemyTag = "Enemies"; // Tag-ul obiectelor inamice
    [SerializeField]
    private float detectionRadius = 10f; // Raza de detectare a inamicilor
    [SerializeField]
    private float rotationSpeed = 10f; // Viteza de rotație

    void Update()
    {
        // Găsește cel mai apropiat inamic din raza de detectare
        GameObject closestEnemy = GetClosestEnemy();

        if (closestEnemy != null)
        {
            // Calculează direcția către cel mai apropiat inamic
            Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;

            // Ignoră axa Y pentru rotație doar pe planul XZ
            direction.y = 0;

            // Calculează rotația necesară
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Aplică rotația graduală
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime * 100f
            );

            // Verifică dacă player-ul este aliniat complet cu inamicul
            float angle = Quaternion.Angle(transform.rotation, lookRotation);
            if (angle < 1f)
            {
                Debug.Log("Player is perfectly aligned with the enemy!");
            }
        }
    }

    // Găsește cel mai apropiat inamic din raza de detectare
    private GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Obține toate obiectele din raza de detectare
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(enemyTag))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.gameObject;
                }
            }
        }

        return closestEnemy;
    }

    // Opțional: vizualizare a razei de detectare în editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
