using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject levelUpPanel;
    public playerControl player; // Inspector'dan Player objesini sürükle

    public void OpenUpgradePanel()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    // Buton 1: Hareket Hızı Artışı
    public void UpgradeSpeed()
    {
        player.moveSpeed += 1.5f; // Hızı 1.5 birim artır
        Debug.Log("Yeni Hız: " + player.moveSpeed);
        ClosePanel();
    }

    // Buton 2: Ateş Hızı (Fire Rate) Artışı
    public void UpgradeFireRate()
    {
        // FireRate bekleme süresi olduğu için onu azaltmak daha hızlı ateş etmektir
        player.fireRate -= 0.03f; 
        if (player.fireRate < 0.05f) player.fireRate = 0.05f; // Sınır koyalım ki makine tüfeğe dönmesin
        Debug.Log("Yeni Ateş Hızı: " + player.fireRate);
        ClosePanel();
    }

    // Buton 3: Hasar Artışı
    public void UpgradeDamage()
    {
        player.damageBoost += 0.5f; // Hasar çarpanını %50 artırır
        Debug.Log("Yeni Hasar Çarpanı: " + player.damageBoost);
        ClosePanel();
    }

    void ClosePanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}