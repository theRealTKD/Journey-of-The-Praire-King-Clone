using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerControl : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;
    public float fireRate = 0.2f;
    public float damageBoost = 1f;
    [Header("Otomatik Atış Ayarları")]
    public float detectionRange = 7f;
    public LayerMask enemyLayer;

    private Vector2 moveInput;
    private float nextFireTime;

    [Header("Ses Ayarları")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    public void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            // Havuzda pasif olanları (ölenleri) es geç
            if (!hit.gameObject.activeInHierarchy) continue;

            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = hit.gameObject;
            }
        }
        return nearest;
    }

    void Shoot(Vector3 targetPos)
    {
        Vector3 shootDir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Havuzdan mermiyi çek
        GameObject bullet = BulletPooler.Instance.GetPooledBullet();
        if (bullet != null) 
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = rotation;
            bullet.SetActive(true);
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Die(); //if you comment this u are immortal
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}