using UnityEngine;

public class XPGem : MonoBehaviour
{
    public float moveSpeed = 5f; // Sabit sürüklenme hızı
    public float collectDistance = 3f; // Oyuncuya ne kadar yaklaşınca çekilmeye başlasın?
    public int xpAmount = 10; // Bu kristal ne kadar XP veriyor?

    private Transform player;
    private bool isFollowing = false;

    void Start()
    {
        // "Player" tagine sahip objeyi bulur
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Mesafe yeterince kısaysa takibe başla
        if (distanceToPlayer <= collectDistance)
        {
            isFollowing = true;
        }

        if (isFollowing)
        {
            // Sabit hızla oyuncuya doğru hareket et
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Oyuncuya çok yaklaştıysa (dokunduysa) toplanmış sayılır
            if (distanceToPlayer < 0.2f)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        // Oyuncu üzerindeki PlayerLevel scriptine ulaşıp XP ekleyeceğiz
        player.GetComponent<PlayerLevel>()?.AddXP(xpAmount);
        Destroy(gameObject);
    }
}