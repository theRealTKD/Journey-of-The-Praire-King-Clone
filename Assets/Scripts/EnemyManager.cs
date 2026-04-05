using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [SerializeField] private List<Enemy> activeEnemies = new List<Enemy>();
    [SerializeField] private LayerMask enemyLayer; // Müfettiş (Inspector) panelinden "Enemy" seçilmeli
    
    private Transform player;
    private Collider2D[] closeNeighbors = new Collider2D[5]; 
    
    [Header("Movement Settings")]
    [SerializeField] private float separationDistance = 0.6f;
    [SerializeField] private float separationWeight = 2f;

    void Awake()
    {
        Instance = this;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

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
            Vector2 moveDirection = ((Vector2)player.position - currentPos).normalized;
            Vector2 separationForce = Vector2.zero;

            // OPTİMİZASYON: Sadece 'enemyLayer' içindeki objeleri tara
            int neighborCount = Physics2D.OverlapCircleNonAlloc(
                currentPos, 
                separationDistance, 
                closeNeighbors, 
                enemyLayer
            );

            for (int j = 0; j < neighborCount; j++)
            {
                Collider2D neighbor = closeNeighbors[j];
                // Kendisini listeden elemek için basit referans kontrolü
                if (neighbor.gameObject != e.gameObject)
                {
                    Vector2 diff = currentPos - (Vector2)neighbor.transform.position;
                    float distSqr = diff.sqrMagnitude; // Performans için: Karekök almayan uzaklık

                    if (distSqr > 0 && distSqr < separationDistance * separationDistance)
                    {
                        // Uzaklığın karesine bölmek, çok yakın olanların daha sert itilmesini sağlar
                        separationForce += diff.normalized / (distSqr + 0.1f);
                    }
                }
            }

            Vector2 finalMovement = moveDirection + (separationForce * separationWeight);
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