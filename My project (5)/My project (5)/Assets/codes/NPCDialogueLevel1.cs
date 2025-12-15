using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueLevel1 : MonoBehaviour
{
    public Dialogue dialogueManager; //a variable that stores the Dialogue script that is attached to the Dialogue Manager gameObject 
   
    //This can be used as a more elegant alternative to FindObjectOfType‹Dialogue>()
    
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Player") //if the player is the one that triggers the collider, then
        { 
            string[] dialogue = { "AVA: Who are you ..",
            "Echo: I am Echo , i will guide you. ",
            "AVA : Where am i ??",
            "ECHO : System Breach detected ... you were sucked inside this corrupted world  ",
            "Echo : To return, you must reach the Core Server and fight the mastermind ",
            "Ava: How ? ",
            "Echo : Use the keys as image and to move forward. Ava,This is where it begins.” ",}; //specify the dialogue between the player and the character
            dialogueManager.SetSentences(dialogue); //set the sentences array in the Dialogue script to above array 
            dialogueManager.StartCoroutine(dialogueManager.TypeDialogue()); //start the coroutine of TypeDialogue), which in turn starts the dialogue
            Destroy (GetComponent<BoxCollider2D>(), 5f); //destroys the NPC's triggered box collider so
            //the player doesn't accidentally re-trigger the conversation 
        }
    }

}



