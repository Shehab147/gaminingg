using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueVictory : MonoBehaviour
{
    public Dialogue dialogueManager; //a variable that stores the Dialogue script that is attached to the Dialogue Manager gameObject 
   
    //This can be used as a more elegant alternative to FindObjectOfType‹Dialogue>()
    
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Player") //if the player is the one that triggers the collider, then
        { 
            string[] dialogue = { "Echo: AVA..YOU DID IT  ",
            "AVA: It's finally over ",
            "Echo: Your path back is stabilizing ",
            "AVA : You stayed with me ,guided me , and assured me ",
            "ECHO : That was my purpose  ",
            "AVA :How  ? ",
            "Echo:  I am you , your conscioussnes, your will to survive  ",
            "Echo : you were never weak.You only forgot how strong you are.” ",
            "AVA :  I BELIEVE IN MYSELF ",
            "Echo : Goodbye , WAKE UP ",}; //specify the dialogue between the player and the character
            dialogueManager.SetSentences(dialogue); //set the sentences array in the Dialogue script to above array 
            dialogueManager.StartCoroutine(dialogueManager.TypeDialogue()); //start the coroutine of TypeDialogue), which in turn starts the dialogue
            Destroy (GetComponent<BoxCollider2D>(), 5f); //destroys the NPC's triggered box collider so
            //the player doesn't accidentally re-trigger the conversation 
        }
    }

}


