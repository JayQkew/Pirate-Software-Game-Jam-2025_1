using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject smallEnemyPrefab; // Prefab for small enemy
    public GameObject bigEnemyPrefab;   // Prefab for big enemy
    public Transform spawnPoint;        // Single spawn point
    public float initialSpawnRate = 2f; // Time between spawns at the start
    public float spawnRateIncrease = 0.1f; // How much the spawn rate increases over time
    public float minSpawnRate = 0.5f;   // Minimum spawn rate to prevent overloading

    private float currentSpawnRate;
    private float spawnTimer;

    private void Start()
    {
        currentSpawnRate = initialSpawnRate;
        spawnTimer = currentSpawnRate;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemies();
            spawnTimer = currentSpawnRate;

            // Gradually increase the spawn rate
            currentSpawnRate = Mathf.Max(minSpawnRate, currentSpawnRate - spawnRateIncrease);
        }
    }

    private void SpawnEnemies()
    {
        int spawnType = Random.Range(0, 3); // Decide what kind of spawn this is
        if (spawnType == 0)
        {
            // Spawn a single small enemy
            SpawnEnemy(smallEnemyPrefab);
        }
        else if (spawnType == 1)
        {
            // Spawn a single big enemy (less frequent)
            SpawnEnemy(bigEnemyPrefab);
        }
        else
        {
            // Spawn a group (small enemies with a chance for one big enemy)
            int groupSize = Random.Range(2, 5); // Randomize group size
            for (int i = 0; i < groupSize; i++)
            {
                SpawnEnemy(smallEnemyPrefab);
            }

            // Random chance to include a big enemy in the group
            if (Random.value < 0.3f) // 30% chance for a big enemy in the group
            {
                SpawnEnemy(bigEnemyPrefab);
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        // Instantiate the enemy at the single spawn point
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
