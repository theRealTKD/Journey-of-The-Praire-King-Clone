using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerControl : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;
    public float fireRate = 0.2f;
    public float damageBoost = 1f;
    public float detectionRange = 7f;

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            // KRİTİK: Sadece havuzda aktif (yaşayan) olan düşmanları tara!
            if (enemy.activeInHierarchy) 
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDistance && distance <= detectionRange)
                {
                    shortestDistance = distance;
                    nearest = enemy;
                }
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

    // playerControl.cs içinde eski OnCollisionEnter2D'yi bununla değiştir:
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Die();
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