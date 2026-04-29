using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public Slider xpSlider;
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
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);//Gereken xp her seviye 1.2 katına çıkıyor yani %20 artıyor
        
        if (upgradeManager != null){
            upgradeManager.OpenUpgradePanel();//Upgrade panelini açarak seviye için gerekli statları seçtir
        }
    }
}