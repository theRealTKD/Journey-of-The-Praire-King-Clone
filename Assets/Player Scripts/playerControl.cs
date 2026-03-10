using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerControl : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] private GameObject bulletPrefab;
    public float moveSpeed = 5f;
    public float fireRate = 0.2f;
    public float damageBoost = 1f;
    public float detectionRange = 7f; // Otomatik atış menzili

    private Vector2 moveInput;
    private float nextFireTime;

    [Header("Ses Ayarları")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    // Movement için gelen sinyali dinle
    public void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Not: OnAttack (Ok tuşları) artık manuel ateş için gerekmiyor, 
    // ama istersen input sinyalini başka işler için tutabilirsin.

    void Update()
    {
        MovePlayer();
        HandleAutoShoot();
    }

    void MovePlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    void HandleAutoShoot()
    {
        // Ateş etme süresi geldi mi?
        if (Time.time >= nextFireTime)
        {
            GameObject target = FindNearestEnemy();

            if (target != null)
            {
                Shoot(target.transform.position);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= detectionRange)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }
        return nearest;
    }

    void Shoot(Vector3 targetPos)
    {
        // Hedefe giden açıyı hesapla
        Vector3 shootDir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        // Mermiyi oluştur
        Instantiate(bulletPrefab, transform.position, rotation);

        // SES ÇALDIRMA
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Öldün!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Menzili sahnede görmek için
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}