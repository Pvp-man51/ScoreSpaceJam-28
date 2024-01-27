using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int Health = 10;
    [SerializeField] private int MaxHealth = 10;

    protected virtual void ReciveDamage(int damage) 
    {
        print("Damage to: " + gameObject.name + "\nAmount: " + damage);

        Health -= damage;

        if (Health < 1) 
        {
            Death();
            return;
        }
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
