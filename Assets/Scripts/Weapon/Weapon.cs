using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("WeaponStuff")]
    [SerializeField] private float TimeBetweenShots = 1.0f;
    [SerializeField] private float Spread = 1.0f;
    [SerializeField] private int BulletAmount = 1;
    [SerializeField] private float BulletSpeed = 3.0f;
    [SerializeField] private bool canPressDown = false;

    private float timeBetweenShotsTimer;

    [Header("Bullet")]
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform ShootPoint;

    private GameObject bulletCointainer;

    private void OnEnable() => PlayerController.onShoot += Shoot;
    private void OnDisable() => PlayerController.onShoot -= Shoot;

    protected void Start()
    {
        timeBetweenShotsTimer = TimeBetweenShots;

        bulletCointainer = GameObject.FindGameObjectWithTag("BulletContainer");
    }

    protected virtual void FixedUpdate()
    {
        LookAtMouse();
    }

    protected virtual void Update()
    {
        UpdateTimers();
    }

    protected virtual void UpdateTimers()
    {
        if (timeBetweenShotsTimer > 0)
            timeBetweenShotsTimer -= Time.deltaTime;
    }

    protected virtual void Shoot()
    {
       if (timeBetweenShotsTimer < 0) 
       { 
            for (int i = 0; i < BulletAmount; i++)
            {
                // Instanciate Bullet obj inculuding it's Rigidbody2D
                GameObject bullet = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                // Add Bullet to BulletCointainer
                if (bulletCointainer != null)
                {
                    bullet.transform.parent = bulletCointainer.transform;
                }

                // Set Direction
                Vector2 dir = transform.rotation * Vector2.right;
                Vector2 pDir = Vector2.Perpendicular(dir) * Random.Range(-Spread, Spread);

                // Apply Velocity
                bulletRb.velocity = (dir + pDir) * BulletSpeed;
            }

            // Reset Timer
            timeBetweenShotsTimer = TimeBetweenShots;
       }
    }

    private void LookAtMouse()
    {
        // Get MousePos and Rotation Value
        Vector2 mousePos = GameManager.Instance.Cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rotation = mousePos - (Vector2)transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        // Apply Rotation
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public bool GetCanPressDown()
    {
        return canPressDown;
    }
}
