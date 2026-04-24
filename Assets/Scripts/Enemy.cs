using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Ayarlar")]
    public float speed = 2f; // Manager'ın okuması için 'public' yaptık
    [SerializeField] private float baseHealth = 1f; 
    [SerializeField] private GameObject xpPrefab; 

    public float currentHealth; // 'public' yaptık, Manager veya Mermi görebilsin
    private SpriteRenderer spriteRenderer;
    private float initialSpeed;
    public float damage = 10;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSpeed = speed;
    }

    void OnEnable()
    {
        // Zorluk ve Renk hesaplamaların aynen kalıyor...
        EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();
        float adilSure = (spawner != null) ? spawner.activeGameTime : 0f;

        speed = initialSpeed;
        float difficultyBonus = Mathf.Floor(adilSure / 60f) * 0.5f;
        currentHealth = baseHealth + difficultyBonus;

        if (adilSure >= 420f) { spriteRenderer.color = Color.black; speed += 3f; }
        else if (adilSure >= 300f) { spriteRenderer.color = Color.red; speed += 2f; }
        else if (adilSure >= 120f) { spriteRenderer.color = Color.yellow; speed += 1.5f; }
        else { spriteRenderer.color = Color.white; }

        // --- YENİ SATIR ---
        // Manager'a "Ben doğdum, beni listeye ekle" diyoruz
        if (EnemyManager.Instance != null) EnemyManager.Instance.AddEnemy(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bulletScript = other.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                TakeDamage(bulletScript.damage);
                bulletScript.Deactivate(); 
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (xpPrefab != null) Instantiate(xpPrefab, transform.position, Quaternion.identity);

        // --- YENİ SATIR ---
        // Manager'a "Ben öldüm, beni listeden çıkar" diyoruz
        if (EnemyManager.Instance != null) EnemyManager.Instance.RemoveEnemy(this);

        gameObject.SetActive(false); 
    }
}