using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Transform player;

    void Start()
    {
        // Sahnedeki "Player" etiketli objeyi bul
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Oyuncuya giden yönü hesapla ve o yöne yürü
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    // Trigger yerine Collision (Çarpışma) kullanıyoruz
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Mermi ile çarpışma kontrolü (Merminin de Is Trigger'ı kapalı olmalı)
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); 
            Destroy(gameObject); 
        }
    }
}