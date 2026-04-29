using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Ayarlar")]
    public float speed = 2f;
    [SerializeField] private float baseHealth = 1f; 
    [SerializeField] private GameObject xpPrefab; 

    public float currentHealth;
    private SpriteRenderer spriteRenderer;
    private float initialSpeed;
    public float damage = 10;
    public Health health;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSpeed = speed;
    }

    void OnEnable()
    {
        EnemySpawner spawner = Object.FindAnyObjectByType<EnemySpawner>();
        float adilSure = (spawner != null) ? spawner.activeGameTime : 0f;

        speed = initialSpeed;
        float difficultyBonus = Mathf.Floor(adilSure / 60f) * 0.5f;
        health.maxHealth = baseHealth + difficultyBonus;
        health.currentHealth = health.maxHealth;

        if (adilSure >= 420f) { spriteRenderer.color = Color.black; speed += 3f; }
        else if (adilSure >= 300f) { spriteRenderer.color = Color.red; speed += 2f; }
        else if (adilSure >= 120f) { spriteRenderer.color = Color.yellow; speed += 1.5f; }
        else { spriteRenderer.color = Color.white; }

        if (EnemyManager.Instance != null) EnemyManager.Instance.AddEnemy(this);

        //burası da event dinleme için
        health.OnDeath.AddListener(Die);
        //health.OnHealthChange.AddListener(UpdateHealthUI);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bulletScript = other.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                health.TakeDamage(bulletScript.damage);
                bulletScript.Deactivate();
            }
        }
    }

    void Die()
    {
        if (xpPrefab != null) Instantiate(xpPrefab, transform.position, Quaternion.identity);

        if (EnemyManager.Instance != null) EnemyManager.Instance.RemoveEnemy(this);

        gameObject.SetActive(false); 
    }

    void OnDisable()
    {
        health.OnDeath.RemoveListener(Die);
        //health.OnHealthChange.RemoveListener(UpdateHealthUI);
    }
}