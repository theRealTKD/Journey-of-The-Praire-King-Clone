using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    public static BulletPooler Instance;

    public GameObject bulletPrefab;
    public int poolSize = 50;
    
    [SerializeField] private List<GameObject> pooledBullets = new List<GameObject>();//GameObjectleri tutacak bi liste

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pooledBullets.Add(obj);
        }
    }

    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];//Bulduğumuz aktif olmayan ilk mermiyi döndür
            }
        }
        // Havuz yetmezse yeni mermi oluştur ve ekle
        GameObject obj = Instantiate(bulletPrefab);
        obj.SetActive(false);
        pooledBullets.Add(obj);//objeyi listemize ekle
        return obj;
    }
}