using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek oyuncu
    public Vector3 offset = new Vector3(0, 0, -10f); // Kameranın derinliği (Z ekseni)

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}