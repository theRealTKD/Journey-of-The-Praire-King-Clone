using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float spawnDistance = 10f; // Ekranın ne kadar uzağında doğsunlar

    void Start()
    {
        // Belirli aralıklarla "SpawnEnemy" fonksiyonunu çağır
        InvokeRepeating("SpawnEnemy", 0f, spawnRate);
    }

    void SpawnEnemy()
    {
        // Rastgele bir açı seç (0-360 derece)
        float angle = Random.Range(0f, Mathf.PI * 2);
        
        // Bu açıya göre ekranın dışında bir nokta hesapla
        Vector3 spawnPos = new Vector3(
            Mathf.Cos(angle) * spawnDistance,
            Mathf.Sin(angle) * spawnDistance,
            0
        );

        // Düşmanı o noktada oluştur
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}