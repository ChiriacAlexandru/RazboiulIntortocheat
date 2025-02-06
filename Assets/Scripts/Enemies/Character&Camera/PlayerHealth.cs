using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100; // Viața maximă a jucătorului
    private int currentHealth;

    public event Action OnPlayerDeath; // Eveniment pentru moartea jucătorului
    public event Action<int> OnHealthChanged; // Eveniment pentru schimbarea vieții

    private void Start()
    {
        currentHealth = maxHealth; // Inițializează viața la maxim
        OnHealthChanged?.Invoke(currentHealth); // Notifică UI-ul
    }

    // Metodă pentru a aplica damage jucătorului
    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // Dacă jucătorul e deja mort, ieși

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Asigură că viața rămâne între 0 și max

        Debug.Log("Jucătorul a primit " + damageAmount + " damage. Viață rămasă: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth); // Trimite actualizarea către UI sau alte sisteme

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Metodă pentru vindecare
    public void Heal(int healAmount)
    {
        if (currentHealth <= 0) return; // Dacă jucătorul e mort, nu se poate vindeca

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Limitează viața la maxHealth

        Debug.Log("Jucătorul s-a vindecat cu " + healAmount + ". Viață actuală: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth); // Actualizează UI-ul
    }

    // Metodă pentru moartea jucătorului
    private void Die()
    {
        Debug.Log("Jucătorul a murit!");

        // Dezactivează jucătorul
        gameObject.SetActive(false);

        // Sau încarcă un ecran de Game Over (opțional)
        // SceneManager.LoadScene("GameOverScene");

        // Trimite evenimentul de moarte
        OnPlayerDeath?.Invoke();
    }
}