using UnityEngine;
using TMPro; // TextMeshPro kullanabilmek için şart

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Inspector'dan TimerText objesini buraya sürükle
    private float elapsedTime = 0f;

    void Update()
    {
        // Zamanı saniye saniye artır
        elapsedTime += Time.deltaTime;

        // Zamanı Dakika:Saniye formatına çevir
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        // Ekrana yazdır (00:00 formatında)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // İleride zorluk artırmak istersen bu değeri dışarıdan alabiliriz
    public float GetTime()
    {
        return elapsedTime;
    }
}