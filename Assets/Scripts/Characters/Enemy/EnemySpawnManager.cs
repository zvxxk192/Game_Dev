using UnityEngine;

[Pausable]
public class EnemySpawnManager : MonoBehaviour
{
    [Header("Enemy Instance")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Enemy Spawn Range")]
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 zSpawnRange;

    [Header("Enemy Spawn attribute")]
    [Range(1.0f, 10.0f)]
    [SerializeField] private float spawnCD = 1.0f;
    [Range(1, 10)]
    [SerializeField] private int maxSpawnCount = 1;

    private float lastSpawnTime = 0f;
    private int spawnCount = 0;

    private void Start()
    {
        while (spawnCount < maxSpawnCount)
        {
            SpawnEnemies();
        }
    }
    private void Update()
    {
        float deltaTime = Time.time - lastSpawnTime;

        while (spawnCount < maxSpawnCount)
        {
            if (deltaTime > spawnCD)
            {
                SpawnEnemies();

                lastSpawnTime = Time.time;

                Debug.Log($"[EnemySpawnManager]: 成功生成敵人，現在場上敵人{spawnCount}");
            }
        }
    }


    private void EnemyDead(EnemyController enemyController)
    {
        spawnCount--;
        enemyController.OnEnemyDead -= EnemyDead;
    }
    private void SpawnEnemies()
    {
        spawnCount++;
        float randomPositionX = Random.Range(xSpawnRange.x, xSpawnRange.y);
        float randomPositionZ = Random.Range(zSpawnRange.x, zSpawnRange.y);
        Vector3 spawnPosition = new Vector3(randomPositionX, 1, randomPositionZ);
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemyInstance.name = enemyPrefab.name;
        EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();
        enemyController.OnEnemyDead += EnemyDead;
    }
}
