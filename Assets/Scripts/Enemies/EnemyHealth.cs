using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] private int health = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}







