using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [SerializeField] private List<Enemy> activeEnemies = new List<Enemy>();
    private Transform player;

    // Performans için önceden ayrılmış dizi (Her karede yeni dizi oluşturmaz)
    private Collider2D[] closeNeighbors = new Collider2D[5]; 
    [SerializeField] private float separationDistance = 0.7f;
    [SerializeField] private float separationWeight = 1.5f;

    void Awake()
    {
        Instance = this;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    // Fizik tabanlı hareketler FixedUpdate içinde yapılmalıdır
    void FixedUpdate()
    {
        if (player == null) return;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Enemy e = activeEnemies[i];

            if (e == null || !e.gameObject.activeInHierarchy)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            Rigidbody2D rb = e.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            Vector2 currentPos = e.transform.position;
            
            // 1. Oyuncuya gidiş yönü
            Vector2 moveDirection = ((Vector2)player.position - currentPos).normalized;

            // 2. PERFORMANSLI AYRILMA (Sadece çok yakındaki 5 komşuya bakar)
            Vector2 separationForce = Vector2.zero;
            int neighborCount = Physics2D.OverlapCircleNonAlloc(currentPos, separationDistance, closeNeighbors);

            for (int j = 0; j < neighborCount; j++)
            {
                Collider2D neighbor = closeNeighbors[j];
                if (neighbor.gameObject != e.gameObject)
                {
                    Vector2 diff = currentPos - (Vector2)neighbor.transform.position;
                    float dist = diff.magnitude;
                    if (dist > 0)
                    {
                        // Yakınlık arttıkça itme kuvveti artar
                        separationForce += diff.normalized / dist;
                    }
                }
            }

            // 3. Kuvvetleri Birleştir
            // .normalized yapmıyoruz ki ayrılma kuvveti gerektiğinde oyuncuya gitme isteğini yenebilsin
            Vector2 finalMovement = moveDirection + (separationForce * separationWeight);
            
            // 4. Hızı Uygula
            rb.linearVelocity = finalMovement.normalized * e.speed;
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        if (!activeEnemies.Contains(enemy)) activeEnemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (activeEnemies.Contains(enemy)) activeEnemies.Remove(enemy);
    }
}