using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Actor
{
    public static PlayerController Instance;

    [Header("Movement")]
    [SerializeField] private float MaxRun = 10f;

    private bool facingRight = true;
    private Vector2 direction;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;

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

        animator.SetBool("Walking", direction.normalized.y != 0 || direction.normalized.x != 0);
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

        if ((dir.x > 0 && !facingRight) || (dir.x < 0 && facingRight))
        {
            Flip();
        }

        // Apply Velocity
        rb.velocity = dir.normalized * MaxRun * pen;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        sr.flipX = !facingRight; 
    }

    #endregion

    protected override void Death()
    {
        if (GameManager.Instance.State == GameState.Death)
            return;

        GameManager.Instance.UpdateGameState(GameState.Death);
    }

    protected override void ReciveDamage(int damage)
    {
        print("Player recived damage");
        Health -= damage;

        if (Health < 1)
        {
            Death();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            ReciveDamage(collision.GetComponent<Enemy>().GetDamage());
        }
    }
}
