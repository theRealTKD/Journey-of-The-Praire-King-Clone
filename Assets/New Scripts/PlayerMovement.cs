using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;

    private PlayerInputHandler inputHandler;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(inputHandler.MoveInput.x, inputHandler.MoveInput.y, 0);
        
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }
}