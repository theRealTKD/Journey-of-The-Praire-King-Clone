using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType { XP, Coin }
    public CollectableType type;

    [Header("Hareket Ayarları")]
    public float pullSpeed = 10f;
    public float detectionRange = 3f;
    public int value = 1;

    private Transform player;
    private bool isBeingPulled = false;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange) isBeingPulled = true;

        if (isBeingPulled)
        {
            // Oyuncuya doğru çekil
            transform.position = Vector3.MoveTowards(transform.position, player.position, pullSpeed * Time.deltaTime);

            // Çok yaklaştıysa toplanmış say
            if (distance < 0.2f)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        if (type == CollectableType.XP)
        {
            player.GetComponent<PlayerLevel>().AddXP(value);
        }
        else if (type == CollectableType.Coin)
        {
            // Cüzdanı bul ve parayı ekle
            PlayerWallet wallet = player.GetComponent<PlayerWallet>();
            if (wallet != null)
            {
                wallet.AddCoins(value);
            }
        }

        Destroy(gameObject);
    }
}