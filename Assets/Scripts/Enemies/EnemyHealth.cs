using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 10; // Punctele de viață ale inamicului

    // Metodă pentru a aplica daune inamicului
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount; // Scade punctele de viață
        Debug.Log(gameObject.name + " a primit " + damageAmount + " daune. Viata ramasa: " + health);

        // Verifică dacă inamicul a murit
        if (health <= 0)
        {
            Die();
        }
    }

    // Metodă pentru a gestiona moartea inamicului
    private void Die()
    {
        Debug.Log(gameObject.name + " a murit!");
        Destroy(gameObject); // Distruge inamicul
    }








}
