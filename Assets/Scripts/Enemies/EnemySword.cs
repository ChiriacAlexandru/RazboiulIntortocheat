using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 2; // Daunele provocate de sabie

    private bool isActive = false; // Starea sabiei (activă sau inactivă)

    /// <summary>
    /// Activează sau dezactivează sabia.
    /// </summary>
    /// <param name="active">True pentru a activa sabia, false pentru a o dezactiva.</param>
    public void SetActive(bool active)
    {
        isActive = active;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sabia a intrat în contact cu: " + other.name);

        // Dacă sabia nu este activă, nu face nimic
        if (!isActive) return;

        // Verifică dacă obiectul cu care s-a ciocnit are tag-ul "Player" sau "PlayerArmy"
        if (other.CompareTag("Player") || other.CompareTag("PlayerArmy"))
        {
            // Obține componenta PlayerHealth de pe jucător sau armata jucătorului
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount); // Aplică daune
                Debug.Log($"{other.name} a primit {damageAmount} daune de la {name}.");
            }
        }
    }
}