using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("Level Verileri")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100; // İlk seviye için gereken XP

    public void AddXP(int amount)
    {
        currentXP += amount;

        // Level atlama kontrolü (While döngüsü sayesinde birden fazla level birden atlayabilir)
        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }


    public UpgradeManager upgradeManager; 

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);

        // Paneli aç komutu gönder
        if (upgradeManager != null)
        {
            upgradeManager.OpenUpgradePanel();
        }
    }

    void ApplyLevelUpRewards()
    {
        // Örnek: Her level atladığında oyuncu biraz hızlansın (Eğer PlayerMovement scriptin varsa)
        // GetComponent<PlayerMovement>().moveSpeed += 0.5f;
    }
}