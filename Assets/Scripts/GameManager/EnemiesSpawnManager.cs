using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesSpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnCheckInterval = 3f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private LayerMask spawnBlockingLayers;

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
                Vector3 spawnPosition = GetRandomSpawnPositionNearPlayer();
                if (IsSpawnPositionValid(spawnPosition))
                {
                    SpawnEnemy(spawnPosition);
                    yield return new WaitForSeconds(0.2f);
                }
            }

            CleanEnemyList();
        }
    }

    private Vector3 GetRandomSpawnPositionNearPlayer()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0,
            Random.Range(-spawnRadius, spawnRadius)
        );
        Vector3 spawnPosition = player.transform.position + randomOffset;

        if (Physics.Raycast(spawnPosition + Vector3.up * 50f, Vector3.down, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
        {
            spawnPosition.y = hit.point.y + 0.5f; // Ridică puțin poziția pentru a evita coliziuni sub hartă
        }
        else
        {
            spawnPosition.y = player.transform.position.y + 1f; // Fallback pentru cazul în care raycastul nu găsește teren
        }

        return spawnPosition;
    }

    private bool IsSpawnPositionValid(Vector3 position)
    {
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
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.transform.position, spawnRadius);
        }
    }
}
