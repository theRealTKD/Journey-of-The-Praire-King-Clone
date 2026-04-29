using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Filtreleme")]
    public LayerMask enemyLayer;
    
    [SerializeField] private List<Enemy> activeEnemies = new List<Enemy>();
    private Transform player;

    private Collider2D[] closeNeighbors = new Collider2D[5]; 
    [SerializeField] private float separationDistance = 0.7f;
    [SerializeField] private float separationWeight = 1.5f;

    private ContactFilter2D contactFilter;

    void Awake()
    {
        Instance = this;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        contactFilter.useTriggers = true;
        contactFilter.SetLayerMask(enemyLayer);
        contactFilter.useLayerMask = true;
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
            
            int neighborCount = Physics2D.OverlapCircle(currentPos, separationDistance, contactFilter, closeNeighbors);

            for (int j = 0; j < neighborCount; j++)
            {
                Collider2D neighbor = closeNeighbors[j];
                
                if (neighbor != null && neighbor.gameObject != e.gameObject)//kendi kendini itme
                {
                    Vector2 diff = currentPos - (Vector2)neighbor.transform.position;
                    float dist = diff.magnitude;
                    
                    if (dist > 0)
                    {
                        separationForce += diff.normalized / dist;
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