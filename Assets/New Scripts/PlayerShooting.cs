using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Ateş Ayarları")]
    public float fireRate = 0.2f;
    public float detectionRange = 7f;

    public float damageBoost = 1f;
    public LayerMask enemyLayer;

    [Header("Ses Efektleri")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    private float nextFireTime;

    void Update()
    {
        HandleAutoShoot();
    }

    private void HandleAutoShoot()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject target = FindNearestEnemy();
            if(target != null)
            {
                Shoot(target.transform.position);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach(var hit in hits)
        {
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

    private void Shoot(Vector3 targetPos)
    {
        Vector3 shootDir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(shootDir.y,shootDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = BulletPooler.Instance.GetPooledBullet();
        if(bullet != null)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}