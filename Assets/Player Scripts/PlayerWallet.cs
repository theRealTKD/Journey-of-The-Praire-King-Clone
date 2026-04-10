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

    public bool SpendCoins(int amount)//Henüz sandıkları eklemediğimden bu fonksiyonun işlevi yok
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            UpdateUI();
            return true; //Para yetti
        }
        return false; //Para yetmedi
    }

    void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "$: " + currentCoins.ToString();
        }
    }
}