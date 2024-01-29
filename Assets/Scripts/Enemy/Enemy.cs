using Pathfinding;
using UnityEngine;

public class Enemy : Actor
{
    [Header("Stats")]
    [SerializeField] private int BaseDamage = 1;
    [SerializeField] private float BaseSpeed = 3f;
    [SerializeField] private Animator MutatedAnim;
    [SerializeField] private string[] walkingSoundNames;

    private int damage;
    private float speed;

    [Header("Components")]
    [SerializeField] private AIDestinationSetter AIDestinationSetter;
    [SerializeField] private AIPath AIPath;

    private Transform target;

    private void OnEnable()
    {
        GameManager.onMutateEnemies += MutateEnemy;
        GameManager.OnGameStateChanged += PlayerDeath;
    }
    private void OnDisable() 
    {
        GameManager.onMutateEnemies -= MutateEnemy;
        GameManager.OnGameStateChanged -= PlayerDeath;
    } 
    private void Start()
    {
        target = PlayerController.Instance.transform;

        AIDestinationSetter.target = target;

        ApplyVariables();

        AIPath.maxSpeed = speed;

        srE = GetComponent<SpriteRenderer>();

        nColor = srE.color;
    }

    private void LateUpdate()
    {
        Vector3 playerPos = transform.InverseTransformPoint(target.position);

        if (playerPos.x < 0)
        {
            srE.flipX = false;
        }
        else if (playerPos.x > 0)
        {
            srE.flipX = true;
        }
    }

    private void ApplyVariables()
    {
        speed = BaseSpeed;
        damage = BaseDamage;
    }

    private void PlayerDeath(GameState state)
    {
        if (state == GameState.Death)
        {
            speed = 0;
            damage = 0;

            AIPath.maxSpeed = speed;
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // Apply Damage
            ReciveDamage(collision.GetComponent<Bullet>().GetDamage());

            // Destory Bullet
            collision.GetComponent<Bullet>().RequestBulletDestroy();
        }
    }

    #region Mutating

    private void MutateEnemy()
    {
        print("MUTATING!!");
        damage = GameManager.Instance.GetMutatedDamage(damage);
        speed = GameManager.Instance.GetMutatedSpeed(speed);
        //Health = GameManager.Instance.GetMutatedEnemyHealth(Health);

        print(damage);

        AIPath.maxSpeed = speed;
    }

    #endregion

    protected override void Death()
    {
        base.Death();
        GameManager.Instance.AddHit();
        GameManager.Instance.AddScore(1);
    }

    public void PlayFootStepSound()
    {
        if (GameManager.Instance.State == GameState.Death)
            return;

        string footStep = walkingSoundNames[Random.Range(0, walkingSoundNames.Length)];
        AudioManager.Instance.Play(footStep);
    }
}
