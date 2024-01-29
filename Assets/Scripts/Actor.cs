using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected int MaxHealth = 10;
    [SerializeField] protected int Health = 10;

    protected SpriteRenderer srE;
    protected Color nColor;

    protected virtual void ReciveDamage(int damage)
    {
        print("Damage to: " + gameObject.name + "\nAmount: " + damage);

        Health -= damage;

        if (Health < 1)
        {
            Death();
        }


        CancelInvoke(nameof(RestColor));
        srE.color = Color.white;
        Invoke(nameof(RestColor), 0.05f);
    }

    private void RestColor()
    {
        srE.color = nColor;
    }

    protected virtual void ReciveHealth(int health)
    {
        print("Health to: " + gameObject.name + "\nAmount: " + health);

        if (health >= MaxHealth)
            return;

        Health += health;
    }

    protected virtual void Death()
    {
        print("Died: " + gameObject.name);
        Destroy(gameObject);
    }
}
