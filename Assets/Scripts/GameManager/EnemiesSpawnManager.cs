using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesSpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject player; // Referință la jucător
    [SerializeField] private GameObject[] enemyPrefabs; // Prefab-urile inamicilor
    [SerializeField] private float spawnCheckInterval = 3f; // Intervalul de verificare a spawn-ului
    [SerializeField] private int maxEnemies = 10; // Numărul maxim de inamici simultan
    [SerializeField] private float minSpawnDistance = 15f; // Distanța minimă față de jucător
    [SerializeField] private float maxSpawnDistance = 30f; // Distanța maximă față de jucător
    [SerializeField] private LayerMask spawnBlockingLayers; // Layer-uri care blochează spawn-ul (ex: obstacole)
    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCheckInterval + Random.Range(-1f, 1f));

            while (activeEnemies.Count < maxEnemies)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                if (IsSpawnPositionValid(spawnPosition))
                {
                    SpawnEnemy(spawnPosition);
                    yield return new WaitForSeconds(0.2f); // Pauză între spawn-uri
                }
            }

            CleanEnemyList();
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        Vector3 spawnPosition = player.transform.position + direction * distance;

        // Ajustează Y conform terenului
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition + Vector3.up * 50f, Vector3.down, out hit, 100f, LayerMask.GetMask("Ground")))
        {
            spawnPosition.y = hit.point.y;
        }

        return spawnPosition;
    }


    private bool IsSpawnPositionValid(Vector3 position)
    {
        // Verifică dacă poziția este liberă (nu este blocată de obstacole)
        bool isBlocked = Physics.CheckSphere(position, 2f, spawnBlockingLayers);
        return !isBlocked;
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemy;

        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
            enemy.transform.position = position;
            enemy.SetActive(true);
        }
        else
        {
            enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], position, Quaternion.identity);
        }

        activeEnemies.Add(enemy);
    }

    private void CleanEnemyList()
    {
        // Elimină inamicii null (distruși) din listă
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    // Desenează zona de spawn în Scene view (pentru debug)
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.transform.position, minSpawnDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.transform.position, maxSpawnDistance);
        }
    }
}