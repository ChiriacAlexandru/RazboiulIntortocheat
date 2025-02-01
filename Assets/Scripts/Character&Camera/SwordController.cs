using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private int damageAmount = 2; // Daunele provocate de sabie

    private void OnTriggerEnter(Collider other)
    {
        // Verifică dacă obiectul cu care s-a ciocnit are tag-ul "enemies"
        if (other.CompareTag("Enemies"))
        {
            // Obține componenta EnemyHealth de pe inamic
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount); // Aplică daune inamicului
            }
        }
    }
}
