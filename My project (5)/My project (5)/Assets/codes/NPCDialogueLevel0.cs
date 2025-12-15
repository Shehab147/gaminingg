using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueLevel0 : MonoBehaviour

{
    public Dialogue dialogueManager; //a variable that stores the Dialogue script that is attached to the Dialogue Manager gameObject 
   
    //This can be used as a more elegant alternative to FindObjectOfType‹Dialogue>()
    
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Player") //if the player is the one that triggers the collider, then
        { 
            string[] dialogue = { "Echo: Ava...Welcome .... ",
            "AVA: What is this place?",
            "Echo: This is the Death Maze. Stay alert as time is counting down .Every Second Matters",
            "AVA : WHAT!! AM GOING TO DIE ?",
            "ECHO : CALM DOWN..AND FOCUS . Watch of moving platforms ,they wont wait for you ",
            "AVA : OK.. WHAT ELSE !! ",
            "Echo: Second .. you should break the wall to open different pathes .",
            "AVA : I guess i can do that ",
            "Echo : Choose wisely ,some paths have  objects to increase time or give you more lives ",
            "Echo :  Few signs warns from  exploding traps , as well as flying spikes ",
            "Echo :Final Surprise ...... You will face the MasterMind by the end make Sure to use your punches for a victory ",
            "AVA: Sure its just a click of F .IM READY ",}; //specify the dialogue between the player and the character
            dialogueManager.SetSentences(dialogue); //set the sentences array in the Dialogue script to above array 
            dialogueManager.StartCoroutine(dialogueManager.TypeDialogue()); //start the coroutine of TypeDialogue), which in turn starts the dialogue
            Destroy (GetComponent<BoxCollider2D>(), 5f); //destroys the NPC's triggered box collider so
            //the player doesn't accidentally re-trigger the conversation 
        }
    }

}


