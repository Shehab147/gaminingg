using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Transform destination; //other portal 
    GameObject player;

    private void Awake()
    {
        player=GameObject.FindGameObjectWithTag("Player"); //Access the player
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            if (Vector2.Distance(player.transform.position,transform.position) >0.3f){
             if (player != null && destination != null){

            //player != null - Makes sure the player GameObject was found
            //destination != null - Makes sure you assigned a destination in the Inspector
            player.transform.position=destination.position;
            }
            }
        }
            
    }
}
