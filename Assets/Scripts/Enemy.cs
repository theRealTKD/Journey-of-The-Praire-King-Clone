using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float baseHealth = 1f; 
    [SerializeField] private GameObject xpPrefab; 

    [SerializeField] private float currentHealth;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private float initialSpeed; // Başlangıç hızını saklamak için

    void Awake()
    {
        // Bir kez çalışması yeterli olanları buraya alalım
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSpeed = speed; // Orijinal hızı yedekle
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    // Havuzdan her çıktığında (SetActive(true) olduğunda) burası çalışır
    void OnEnable()
    {
        // 1. ADİL SÜREYİ ÇEK
        EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();
        float adilSure = 0f;
        if (spawner != null) adilSure = spawner.activeGameTime;

        // 2. Hızı her seferinde sıfırla (Yoksa her doğuşta üst üste eklenip uçar gider!)
        speed = initialSpeed;

        // 3. Can ve Zorluk Ayarlarını Güncelle
        float difficultyBonus = Mathf.Floor(adilSure / 60f) * 0.5f;
        currentHealth = baseHealth + difficultyBonus;

        // 4. Renk ve Hız Kontrolü
        if (adilSure >= 420f)
        {
            spriteRenderer.color = Color.black;
            speed += 3f;
        }
        else if (adilSure >= 300f)
        {
            spriteRenderer.color = Color.red;
            speed += 2f;
        }
        else if (adilSure >= 120f)
        {
            spriteRenderer.color = Color.yellow;
            speed += 1.5f;
        }
        else
        {
            // Eğer süre 120'den azsa rengi beyaza (normaline) çekmeyi unutma
            spriteRenderer.color = Color.white;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mermi çarptı mı?
        if (other.CompareTag("Bullet"))
        {
            Bullet bulletScript = other.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                TakeDamage(bulletScript.damage);
                // Mermiyi uyut (Havuza gönder)
                bulletScript.Deactivate(); 
            }
        }
        
        // Oyuncuya çarptı mı? (Düşman oyuncuya değerse oyuncu ölsün)
        if (other.CompareTag("Player"))
        {
            // Oyuncunun içindeki Die() fonksiyonunu çağırabilirsin 
            // veya direkt sahneyi reload edebilirsin
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    [Header("Düşürme Ayarları")]
    [Range(0, 100)] public float coinDropChance = 20f; // %20 ihtimal
    public GameObject coinPrefab;

    void Die()
    {
        // XP ve Coin düşürme kodların burada kalsın (Instantiate ile)
        if (xpPrefab != null) Instantiate(xpPrefab, transform.position, Quaternion.identity);

        // ÖNEMLİ: Obje yok edilmiyor, sadece havuza geri gönderiliyor
        gameObject.SetActive(false); 
    }
}