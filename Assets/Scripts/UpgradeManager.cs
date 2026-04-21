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

    public void OnSpeedBtnClick()//Geçiçi Sistem
    {
        ApplyUpgrade(new SpeedUpgrade());
    }

    public void OnFireRateBtnClick()//Geçiçi Sistem
    {
        ApplyUpgrade(new FireRateUpgrade());
    }

    public void OnDamageBtnClick()//Geçiçi Sistem
    {
        ApplyUpgrade(new DamageUpgrade()); 
    }
    public void ApplyUpgrade(IUpgradeVisitor chosenUpgrade)
    {
        player.Accept(chosenUpgrade);
        ClosePanel();
    }

    void ClosePanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}