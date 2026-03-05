using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f; // Mermi ekrandan çıkarsa silinsin diye

    void Start()
    {
        // Mermi doğduğu an 3 saniye sonra yok olması için zamanlayıcı
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Mermiyi ileri (sağa doğru) sür
        transform.position += transform.right * speed * Time.deltaTime;
    }
}