using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // assign Bullet prefab
    public Transform firePoint;      // assign FirePoint
    public float bulletSpeed = 10f;
    public KeyCode shootKey = KeyCode.F;

    PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // create the bullet at FirePoint position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // give the bullet velocity in the direction the player faces
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 dir = playerController.GetFacingDirection(); // uses your existing method
        rb.velocity = dir * bulletSpeed;
    }
}
