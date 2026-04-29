using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }

    public void OnMovement(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }
}