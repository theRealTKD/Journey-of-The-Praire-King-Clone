using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject levelUpPanel;
    public playerControl player;

    public void OpenUpgradePanel()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    //Hareket Hızı Artışı
    public void UpgradeSpeed()
    {
        player.moveSpeed += 1.5f; // Hızı 1.5 birim artır
        Debug.Log("Yeni Hız: " + player.moveSpeed);
        ClosePanel();
    }

    //Ateş Hızı Artışı
    public void UpgradeFireRate()
    {
        // FireRate bekleme süresi olduğu için onu azaltmak daha hızlı ateş etmektir
        player.fireRate -= 0.03f; 
        if (player.fireRate < 0.01f) player.fireRate = 0.01f; // Negatif olamasın.
        Debug.Log("Yeni Ateş Hızı: " + player.fireRate);
        ClosePanel();
    }

    //Hasar Artışı
    public void UpgradeDamage()
    {
        player.damageBoost += 0.5f; //plus hasar
        Debug.Log("Yeni Hasar Çarpanı: " + player.damageBoost);
        ClosePanel();
    }

    void ClosePanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}