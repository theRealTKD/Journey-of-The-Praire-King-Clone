using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject xpPrefab; // Inspector'dan XPGem prefabını buraya sürükle
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Ölmeden hemen önce XP oluştur
            if (xpPrefab != null)
            {
                Instantiate(xpPrefab, transform.position, Quaternion.identity);
            }

            Destroy(collision.gameObject); 
            Destroy(gameObject); 
        }
    }
}