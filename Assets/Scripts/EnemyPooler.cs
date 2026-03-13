using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public static EnemyPooler Instance;

    [Header("Havuz Ayarları")]
    public GameObject enemyPrefab;
    public int poolSize = 200;
    
    [SerializeField] private List<GameObject> pooledEnemies = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Oyun başında düşmanları oluşturup kapalı (false) halde listeye ekle
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pooledEnemies.Add(obj);
        }
    }

    public GameObject GetPooledEnemy()
    {
        // Havuzda boşta yatan düşman var mı bak
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (!pooledEnemies[i].activeInHierarchy)
            {
                return pooledEnemies[i];
            }
        }

        // Havuz yetmezse yeni bir tane oluşturup listeye ekle (Genişleyen Havuz)
        GameObject obj = Instantiate(enemyPrefab);
        obj.SetActive(false);
        pooledEnemies.Add(obj);
        return obj;
    }

    private void OnGUI()//DEBUG FUNC
    {
    // Ekranın sol üstünde havuz durumunu yazdırır
    GUI.Label(new Rect(10, 50, 200, 50), "Havuzdaki Toplam: " + pooledEnemies.Count);
    }
}