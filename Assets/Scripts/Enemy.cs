using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float baseHealth = 1f; 
    [SerializeField] private GameObject xpPrefab; 

    private float currentHealth;
    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 1. Oyuncuyu Bul
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // 2. ADİL SÜREYİ ÇEK (activeGameTime)
        // Spawner scriptindeki o durabilen süreyi buluyoruz
        EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();
        float adilSure = 0f;

        if (spawner != null)
        {
            // Spawner'daki activeGameTime değişkenine ulaşıyoruz
            // (Not: EnemySpawner içinde activeGameTime'ın "public" olduğundan emin ol)
            adilSure = spawner.activeGameTime; 
        }

        // 3. Can Hesaplama (Adil Süreye Göre)
        float difficultyBonus = Mathf.Floor(adilSure / 60f) * 0.5f;
        currentHealth = baseHealth + difficultyBonus;

        // 4. Renk Kontrolü (5. Dakika = 300 Saniye)

        if (adilSure >= 420f)
        {
            spriteRenderer.color = Color.black;
            speed += 1.5f;
        }
        else if (adilSure >= 300f)
        {
            spriteRenderer.color = Color.red;
            speed += 1.5f;
        }
        else if (adilSure >= 120f)
        {
            spriteRenderer.color = Color.yellow;
            speed += 1.5f;
        }
        
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bulletScript = collision.gameObject.GetComponent<Bullet>();
            float damageTaken = (bulletScript != null) ? bulletScript.damage : 1f;

            TakeDamage(damageTaken);
            Destroy(collision.gameObject); 
        }
    }

    void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    [Header("Düşürme Ayarları")]
    [Range(0, 100)] public float coinDropChance = 20f; // %20 ihtimal
    public GameObject coinPrefab;

    void Die()
    {
        // Her zaman XP düşür
        if (xpPrefab != null) Instantiate(xpPrefab, transform.position, Quaternion.identity);

        // Şans kontrolü ile Coin düşür
        float randomRoll = Random.Range(0f, 100f);
        if (randomRoll <= coinDropChance && coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}