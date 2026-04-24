using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerControl : MonoBehaviour, IAcceptUpgrades
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;
    public float fireRate = 0.2f;
    public float damageBoost = 1f;
    public float detectionRange = 7f;
    public LayerMask enemyLayer;

    private Vector2 moveInput;
    private float nextFireTime;

    [Header("Ses Ayarları")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    [Header("Can Ayarları")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float invincibilityTime = 0.5f;
    private float nextDamageTime;

    public void Accept(IUpgradeVisitor visitor)
    {
        visitor.Visit(this);
    }
    public void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        MovePlayer();
        HandleAutoShoot();

        // TEST İÇİN: 1 tuşuna basınca hız, 2 tuşuna basınca ateş hızı artar
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Accept(new SpeedUpgrade());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Accept(new FireRateUpgrade());
        }
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
                //Shoot(target.transform.position);
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
            // Havuzda pasif olanları es geç
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

    public void TakeDamage(float damage)
    {
        if(Time.time < nextDamageTime)
        {
            return;
        }

        currentHealth -= damage;
        nextDamageTime = Time.time + invincibilityTime;

        Debug.Log("Can azaldı! Kalan canın: "+ currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();

            if(enemyScript != null)
            {
                TakeDamage(enemyScript.damage);
            }
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