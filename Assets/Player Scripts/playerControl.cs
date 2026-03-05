using UnityEngine;
using UnityEngine.InputSystem;

public class playerControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;

    // "Movement" Action ismiyle eşleşmesi için "On" + "Movement" = OnMovement
    public void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // Convert movement input to a Vector3 (X, Y, Z)
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);
        
        // Apply movement
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        // Sprite Flipping (Facing Direction)
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}