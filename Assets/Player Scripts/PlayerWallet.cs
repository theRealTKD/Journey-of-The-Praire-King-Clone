using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    public int currentCoins = 0;
    public TextMeshProUGUI coinText;

    void Start()
    {
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            UpdateUI();
            return true; // Para yetti, işlem tamam
        }
        return false; // Para yetmedi!
    }

    void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "$: " + currentCoins.ToString();
        }
    }
}