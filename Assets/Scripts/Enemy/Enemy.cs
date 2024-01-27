using Pathfinding;
using UnityEngine;

public class Enemy : Actor
{
    [Header("Stats")]
    [SerializeField] private int BaseDamage = 1;
    [SerializeField] private float BaseSpeed = 3f;
    [SerializeField] private int MutationLevelCap = 3;

    private int damage;
    private float speed;
    private int mutationLevel;

    [Header("Components")]
    [SerializeField] private AIDestinationSetter AIDestinationSetter;
    [SerializeField] private AIPath AIPath;

    private void OnEnable() => GameManager.onMutateEnemies += MutateEnemy;
    private void OnDisable() => GameManager.onMutateEnemies -= MutateEnemy;


    private void Start()
    {
        AIDestinationSetter.target = PlayerController.Instance.transform;

        ApplyVariables();

        AIPath.maxSpeed = speed;
    }

    private void ApplyVariables()
    {
        speed = BaseSpeed;
        damage = BaseDamage;
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

        print(damage);

        AIPath.maxSpeed = speed;
    }

    #endregion
}
