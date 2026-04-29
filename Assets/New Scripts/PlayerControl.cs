using System;
using UnityEngine;

public class playerControl : MonoBehaviour, IAcceptUpgrades
{
    // KASLAR (Bileşen Referansları)
    [Header("Modüler Sistemler")]
    public Health health;
    public PlayerMovement movement;
    public PlayerShooting shooting;

    private void Awake()
    {
        if (health == null) health = GetComponent<Health>();
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (shooting == null) shooting = GetComponent<PlayerShooting>();
    }
    public void Accept(IUpgradeVisitor visitor)
    {
        visitor.Visit(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Düşmana temas ettiysen canın azalsın
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                health.TakeDamage(enemyScript.damage);

            }
        }
    }

    void OnEnable()
    {
        health.OnDeath.AddListener(RestartGame);
        health.OnHealthChange.AddListener(UpdateHealthUI);

    }

    void OnDisable()
    {
        health.OnDeath.RemoveListener(RestartGame);
        health.OnHealthChange.RemoveListener(UpdateHealthUI);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void UpdateHealthUI(float current,float max)
    {
        Debug.Log("Can durumu: "+ current + "/" + max);
    }
}