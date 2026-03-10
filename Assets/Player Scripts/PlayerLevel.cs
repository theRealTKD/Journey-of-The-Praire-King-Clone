using UnityEngine;
using UnityEngine.UI; // Slider için gerekli

public class PlayerLevel : MonoBehaviour
{
    public Slider xpSlider; // Inspector'dan XPBar'ı buraya sürükle
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public UpgradeManager upgradeManager;

    void Start()
    {
        UpdateXPBar();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        UpdateXPBar();
    }

    void UpdateXPBar()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = currentXP;
        }
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);
        
        if (upgradeManager != null) upgradeManager.OpenUpgradePanel();
    }
}