using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // auto‑destroy after a few seconds
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SpecterBoss boss = other.GetComponent<SpecterBoss>();
        if (boss != null)
        {
            boss.TakeDamage(damage); // reduce boss health
            Destroy(gameObject);    // remove bullet
        }
    }
}
