using UnityEngine;

public class SpawnerFollow : MonoBehaviour
{
    public Transform playerTransform; // Inspector'dan oyuncuyu buraya sürükle

    void Update()
    {
        if (playerTransform != null)
        {
            // Spawner'ın pozisyonunu her karede oyuncununkine eşitler
            transform.position = playerTransform.position;
        }
    }
}
