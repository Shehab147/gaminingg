using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   
    public float maxSpeed = 2;
    public int damage = 1;
    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    
    public void Flip(){
        sr.flipX = !sr.flipX;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
   
    void OnTriggerEnter2D(Collider2D other){
    // Try to get PlayerController from the collided object
    PlayerController player = other.GetComponent<PlayerController>();
    if (player != null)
    {
        player.TakeDamage((float)damage); // TakeDamage expects float
        Flip();
    }
    else if (other.CompareTag("Wall")){
        Flip();
    }
    }
}
