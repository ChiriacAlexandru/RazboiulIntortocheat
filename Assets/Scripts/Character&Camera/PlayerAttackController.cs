using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField]
    private string enemyTag = "Enemies"; // Tag-ul obiectelor inamice
    [SerializeField] 
    private float attackRadius = 3f; // Raza de atac
    private GameObject currentEnemy; // Referință la inamicul curent
    private Animator animator; // Referință la Animator
    [SerializeField]
    private Rigidbody rb;
    void Start()
    {
        //obtinem rb ul din parinte
        rb = GetComponentInParent<Rigidbody>();

        // Obține referința la Animator de pe obiectul curent
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Găsește cel mai apropiat inamic în raza de atac
        GameObject closestEnemy = GetClosestEnemy();

        if (closestEnemy != null && rb.linearVelocity == Vector3.zero)
        {
            // Salvează referința la inamicul curent
            currentEnemy = closestEnemy;

            // Activează animația de atac
            animator.SetBool("IsAtacking", true);
        }
        else
        {
            // Oprește animația de atac dacă nu mai există inamici
            animator.SetBool("IsAtacking", false);
            currentEnemy = null;
        }
    }

    // Găsește cel mai apropiat inamic în raza de atac
    private GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Obține toate obiectele din raza de atac
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius);

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

    // Opțional: vizualizare a razei de atac în editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
