using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public float damage = 1f;

    void OnEnable()
    {
        CancelInvoke();//önceki lifeTime deaktivasyon çağrısını varsa iptal et
        Invoke("Deactivate", lifeTime);//Deaktivasyon için yeni bi çağrı kur
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Deactivate();//hasarı ve ölmeyi Enemy.cs içerisinde hallediyoruz
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();//Disable edildi ise Deactivate'yi çağırmasın diye. İhtiyacımız var mı buna emin değilim
    }
}