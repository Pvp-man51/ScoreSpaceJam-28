using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Movement")]
    [SerializeField] private float MaxRun = 10f;

    private Vector2 direction;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    public delegate void Shoot();
    public static event Shoot onShoot;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        Movement(direction);
    }

    #region GetInput

    public void OnMovement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context) 
    {
        if (context.action.IsPressed())
        {
            onShoot?.Invoke();
        }
    }

    #endregion

    #region Movement

    private void Movement(Vector2 dir)
    {
        float pen = 1;

        // If the player is moving diagonally, apply a penalty factor to reduce the speed
        if ((dir.x > 0.5f || dir.x < -0.5f) && (dir.y > 0.5f || dir.y < -0.5f))
        {
            pen = 1.35f;
        }

        // Apply Velocity
        rb.velocity = dir * MaxRun * pen;
    }

    #endregion
}
