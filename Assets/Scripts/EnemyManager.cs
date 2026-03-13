using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [SerializeField] private List<Enemy> activeEnemies = new List<Enemy>();
    private Transform player;

    void Awake()
    {
        Instance = this;
        // Oyuncuyu sadece bir kere bulup hafızaya alıyoruz (Performans yipeee!)
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Listeyi sondan başa tarıyoruz (Eleman silerken hata almamak için)
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Enemy e = activeEnemies[i];

            if (e == null || !e.gameObject.activeInHierarchy)
            {
                activeEnemies.RemoveAt(i);//aktif olmayanları çıkar dışarı
                continue;//olmayan bi şeyi hareket ettirmek istemediğimizden devam et.
            }

            // Tek merkezden Hareket
            Vector3 direction = (player.position - e.transform.position).normalized;
            e.transform.position += direction * e.speed * Time.deltaTime;
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        if (!activeEnemies.Contains(enemy)){
            activeEnemies.Add(enemy);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (activeEnemies.Contains(enemy)){
            activeEnemies.Remove(enemy);
        }
    }
}