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

    [Header("Audio")]
    [SerializeField] private string[] walkingSoundNames;

    private bool invincible;
    private int cooldown;
    private int pCooldown;
    private Vector2 pushDir;

    private Color dColor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dColor = sr.color;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == GameState.Death)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        CheckInvincibleStuff();
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

    private void ReciveDamage(Vector2 dir, int damage)
    {
        if (invincible)
            return;

        print("Player recived damage");

        if (Health < 1)
        {
            Death();
            return;
        }

        Health -= damage;

        invincible = true;
        cooldown = 80;
        pCooldown = 25;
        pushDir = -dir;

        CancelInvoke(nameof(ResetColor));
        sr.color = Color.white;
        Invoke(nameof(ResetColor), 0.05f);
    }

    private void ResetColor()
    {
        sr.color = dColor;
    }

    private void CheckInvincibleStuff()
    {
        if (!invincible)
            return;

        float a = Mathf.PingPong(Time.time * 10, 0.6f) + 0.3f;
        if (a < 0.7f)
            sr.color = dColor;
        else
            sr.color = Color.black;

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a / 1.3f);

        cooldown--;
        pCooldown--;

        if (cooldown < 1)
        {
            invincible = false;
        }

        if (pCooldown > 0)
        {
            rb.velocity = pushDir;
            return;
        }

        if (cooldown == 0)
            sr.color = dColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            ReciveDamage(rb.velocity, collision.GetComponent<Enemy>().GetDamage());
        }
    }

    public int GetHealth()
    {
        return Health;
    }

    public void PlayFootStepSound()
    {
        if (GameManager.Instance.State == GameState.Death)
            return;
        string footStep = walkingSoundNames[Random.Range(0, walkingSoundNames.Length)];
        AudioManager.Instance.Play(footStep);
    }
}
