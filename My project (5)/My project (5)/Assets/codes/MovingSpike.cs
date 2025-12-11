using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    //movememnt settings
    public float moveDistance = 2.0f;
    public float speed = 2.0f;
    public float damage = 20f;

    private Vector2 startPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        MoveSlide();
    }
    
    void MoveSlide()
    {
        // Choose target position based on direction 
        Vector2 target = startPos + (movingUp ? Vector2.up * moveDistance : Vector2.down * moveDistance);

        // Move towards target
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch direction when reaching target
        if (Vector2.Distance(transform.position, target) < 0.01f)
        {
            movingUp = !movingUp;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
    
