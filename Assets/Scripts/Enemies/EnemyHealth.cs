using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private GameObject soldierPrefab;
    private float _enemyHealth = 10f;
    public void TakeDamage(float damage)
    {
        _enemyHealth -= damage;
        if (_enemyHealth <= 0)
        {
            Die();
        }
    }
    public void Die() {
        ArmyManager armyManager = FindAnyObjectByType<ArmyManager>();
        if (armyManager != null)
        {
            Soldier newSoldier = Instantiate(soldierPrefab, transform.position, Quaternion.identity).GetComponent<Soldier>();
            armyManager.AddSoldier(newSoldier);
        }

        // Distruge inamicul
        Destroy(gameObject);
    }



}

