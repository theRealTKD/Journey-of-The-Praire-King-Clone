using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    public float damage = 1f; // Yeni: Hasar değişkeni

    void Start()
    {
        // Mermi doğunca oyuncudaki damageBoost miktarını kendi hasarına aktarır
        playerControl player = Object.FindAnyObjectByType<playerControl>();
        if (player != null)
        {
            damage = 1f * player.damageBoost; // Oyuncunun hasar çarpanını uygula
        }
        
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}