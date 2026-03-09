using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerControl : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Mermi şablonu
    public float moveSpeed = 5f;
    public float fireRate = 0.2f;
    public float damageBoost = 1f;
    
    private Vector2 moveInput;
    private Vector2 attackInput;
    private float nextFireTime;

    // Movement için gelen sinyali dinle
    public void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Attack (Ok tuşları) için gelen sinyali dinle
    public void OnAttack(InputValue value)
    {
        attackInput = value.Get<Vector2>();
    }

    void Update()
    {
        MovePlayer();

        // Ateş etme kontrolü
        if (attackInput.sqrMagnitude > 0 && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void MovePlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    void Shoot()
    {
        // Ok tuşlarının açısını hesapla
        float angle = Mathf.Atan2(attackInput.y, attackInput.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        // Mermiyi oluştur
        Instantiate(bulletPrefab, transform.position, rotation);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eğer çarptığımız objenin etiketi "Enemy" ise
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Öldün!");
        
        // Şimdilik en basit yöntem: Sahneyi en baştan yükle (Restart)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}