using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek oyuncu
    public Vector3 offset = new Vector3(0, 0, -10f); // Kameranın derinliği (Z ekseni)

    void LateUpdate()
    {
        if (target != null)
        {
            // Pozisyonu yumuşatma olmadan direkt eşitliyoruz
            transform.position = target.position + offset;
        }
    }
}