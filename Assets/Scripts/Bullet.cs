using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public float damage = 1f; // HATAYI ÇÖZEN SATIR

    void OnEnable()
    {
        CancelInvoke();
        Invoke("Deactivate", lifeTime);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mermi bir şeye çarptığında havuza dönmeli
        // Hasar verme işini artık Enemy.cs içindeki Trigger halledecek
        if (other.CompareTag("Enemy"))
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}