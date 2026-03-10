using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float initialSpawnRate = 2f;
    [SerializeField] private float difficultyCurve = 0.05f;
    [SerializeField] private float minSpawnRate = 0.1f;
    [SerializeField] private float spawnDistance = 30f;

    private float currentSpawnRate;
    public float activeGameTime = 0f;   

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        activeGameTime += Time.deltaTime;
    }

    System.Collections.IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            
            // Zorluğu artık activeGameTime üzerinden hesaplıyoruz
            currentSpawnRate = Mathf.Max(minSpawnRate, initialSpawnRate - (activeGameTime * difficultyCurve));
            
            yield return new WaitForSeconds(currentSpawnRate);
        }
    }

    void SpawnEnemy()
    {
        // Rastgele bir açı seç
        float angle = Random.Range(0f, Mathf.PI * 2);
        
        // Senin belirlediğin spawnDistance değerini kullanarak pozisyonu hesapla
        Vector3 spawnPos = new Vector3(
            Mathf.Cos(angle) * spawnDistance, 
            Mathf.Sin(angle) * spawnDistance, 
            0
        );

        Instantiate(enemyPrefab, transform.position + spawnPos, Quaternion.identity);
    }
}