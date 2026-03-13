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
            //currentSpawnRate = 0.001f; //DON'T
            yield return new WaitForSeconds(currentSpawnRate);
        }
    }

    // EnemySpawner.cs içindeki SpawnEnemy fonksiyonu:

void SpawnEnemy()
{
    // Havuzdan bir düşman iste
    GameObject enemy = EnemyPooler.Instance.GetPooledEnemy();

    if (enemy != null)
    {
        // Doğacağı yeri hesapla (Eski kodundaki mesafe mantığı)
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector3 spawnPos = new Vector3(Mathf.Cos(angle) * spawnDistance, Mathf.Sin(angle) * spawnDistance, 0);

        // Konumunu ve rotasyonunu ayarla
        enemy.transform.position = transform.position + spawnPos;
        enemy.transform.rotation = Quaternion.identity;

        // DÜŞMANI UYANDIR!
        enemy.SetActive(true);
    }
}
}