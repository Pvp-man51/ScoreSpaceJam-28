using Pathfinding;
using UnityEngine;

public class Enemy : Actor
{
    [Header("Stats")]
    [SerializeField] private int Damage = 1;

    [Header("Components")]
    [SerializeField] private AIDestinationSetter aIDestinationSetter;
    
    private void Start()
    {
        aIDestinationSetter.target = PlayerController.Instance.transform;
    }

    public int GetDamage() 
    {
        return Damage;
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
}
