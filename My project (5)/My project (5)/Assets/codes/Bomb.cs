using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public float damage = 10f;
    public AudioClip explosionSound;  
    
    private Animator animator;
    private bool hasExploded = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (hasExploded) return;

    if (other.tag == "Player")
    {
        hasExploded = true;
        animator.SetTrigger("Explosion");
        
         // Play explosion sound effect
            if (explosionSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusicSFX(explosionSound);
            }
        
        //  this line to damage the player
        other.GetComponent<PlayerController>().TakeDamage(damage);
        
        Destroy(gameObject, 1f);
    }
}
}

