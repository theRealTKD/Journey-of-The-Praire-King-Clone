using UnityEngine;
using UnityEngine.Events;

public class Health: MonoBehaviour
{
    [Header("Can Ayarları")]
    public float maxHealth = 100f;
    public float currentHealth = 100;
    public float invincibilityTime = 0.5f;
    private float nextDamageTime;

    //Bu scriptin yayacağı haberler
    //1. Can değerim değişti
    public UnityEvent<float,float> OnHealthChange;

    //2. Canım bitti
    public UnityEvent OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, bool override_invincibility = false)
    {
        //ilk hasarı aldığımız zaman mesela 105. saniyede 105 < 0 yanlış demekki hasar alma işine devam et
        //ve nextDamageTime'ı oyun süresi + invincibletime'a eşitle;
        if (Time.time < nextDamageTime && !override_invincibility) 
        {
            return; //hasar alamaz
        }
        //hasar alabilir
        currentHealth -= damage;
        nextDamageTime = Time.time + invincibilityTime;

        //Can değişti haberini sal
        OnHealthChange?.Invoke(currentHealth, maxHealth);

        if(currentHealth<= 0)
        {
            OnDeath.Invoke();
        }  
    }
    
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth+amount, maxHealth);
        OnHealthChange?.Invoke(currentHealth, maxHealth);
        //soru işareti bu evente abone olan biri yoksa nullreferenceexception almamızı engelliyor
    }

}