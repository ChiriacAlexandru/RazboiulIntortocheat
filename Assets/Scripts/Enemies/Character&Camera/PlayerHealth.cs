using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int regenAmount = 5; // Câtă viață regenerează pe tick
    [SerializeField] private float regenInterval = 1f; // Intervalul regenerării (1 sec)
    [SerializeField] private float regenDelay = 3f; // Cât așteaptă după damage ca să înceapă regenerarea

    private int currentHealth;
    private float lastDamageTime; // Ultimul timp când jucătorul a luat damage
    private Coroutine regenCoroutine; // Referință la regenerare pentru a o opri

    public event Action OnPlayerDeath;
    public event Action<int> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        lastDamageTime = Time.time; // Actualizează timpul ultimei lovituri

        Debug.Log("Jucătorul a primit " + damageAmount + " damage. Viață rămasă: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth);

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine); // Oprește regenerarea dacă ia damage
            regenCoroutine = null;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Începe regenerarea după delay dacă nu moare
            regenCoroutine = StartCoroutine(RegenerateHealth());
        }
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(regenDelay); // Așteaptă 3 secunde înainte de regenerare

        while (currentHealth < maxHealth)
        {
            if (Time.time - lastDamageTime < regenDelay) yield break; // Se oprește dacă ia damage din nou

            currentHealth += regenAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
            Debug.Log("Regenerare... Viață actuală: " + currentHealth);

            yield return new WaitForSeconds(regenInterval); // Așteaptă intervalul de regenerare
        }

        regenCoroutine = null; // Termină regenerarea
    }

    public void Heal(int healAmount)
    {
        if (currentHealth <= 0) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);

        Debug.Log("Jucătorul s-a vindecat cu " + healAmount + ". Viață actuală: " + currentHealth);
    }

    private void Die()
    {
        Debug.Log("Jucătorul a murit!");
        gameObject.SetActive(false);
        OnPlayerDeath?.Invoke();
    }
}
