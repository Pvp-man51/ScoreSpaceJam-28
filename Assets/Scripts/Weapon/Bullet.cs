using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int Damage = 1;
    [SerializeField] private bool RqDestroy = true;
    [SerializeField] private bool DestoryBulletAfterSomeTime = true;
    [SerializeField] private float DestroyTime = 3f;

    private void Start()
    {
        if (DestoryBulletAfterSomeTime)
        {
            Destroy(gameObject, DestroyTime);
        }
    }

    public int GetDamage()
    {
        return Damage;
    }

    public void RequestBulletDestroy() 
    {
        if (RqDestroy)
        {
            print("Bullet Destory Request Suceeded!");
            Destroy(gameObject);
            return;
        }

        print("!!Bullet Destory Request Failed!!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
