using UnityEngine;
using UnityEngine.InputSystem;

public class playerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab; // Mermi şablonu (Inspector'dan sürükle)
    [SerializeField] private float fireRate = 0.2f; 
    
    private Vector2 moveInput;
    private Vector2 attackInput; // Hata veren değişkeni buraya ekledik
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

        // Sprite Flipping (Karakterin yönü)
        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
        }
    }

    void Shoot()
    {
        // Ok tuşlarının açısını hesapla
        float angle = Mathf.Atan2(attackInput.y, attackInput.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
        // Mermiyi oluştur
        Instantiate(bulletPrefab, transform.position, rotation);
    }
}