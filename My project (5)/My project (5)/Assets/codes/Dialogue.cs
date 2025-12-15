using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Dialogue : MonoBehaviour
{
public TextMeshProUGUI textDisplay; //a special variable that holds the TextMeshPro - Text for manipulation 
private string[] dialogueSentences; //an array that stores all the sentences to be displayed
private int index = 0; //a variable that signifies which sentence is being printed or to be printed
public float typingSpeed; //a variable to control the speed of the typewriter effect 
public GameObject continueButton; //a variable that holds the continue button  
public GameObject dialogueBox; //a variable that holds the panel (dialogue box) 
public Rigidbody2D playerRB; //a variable that holds the player's/character's Rigidbody2D component

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        continueButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetSentences(string[] sentences){
        this.dialogueSentences= sentences;
    }


//Enumerator is a special type of function where with the help of the StartCoroutine() function, 
//the I/IEnumerator can be paused and resumed for a specified amount of time without pausing the game itself.
//The purpose of this function is to display the dialogue with a typewriter effect 
    public IEnumerator TypeDialogue(){
         dialogueBox.SetActive(true); //enables the dialogue box
         playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; //freezing the player in place
         foreach (char letter in dialogueSentences[index].ToCharArray()) //converting the sentence to an array of char to loop through each char
         {
            textDisplay.text += letter; //adding each char to the displayed text
            yield return new WaitForSeconds (typingSpeed); //this special type of return is used with the IEnumerator and StartCoroutine() function //to pause the execution of this function (TypeDialogue) for the specified (typingSpeed) amount of seconds, then after this amount //of seconds has passed this function (TypeDialogue) continues its execution
            if (textDisplay.text == dialogueSentences[index]) //checks if the whole sentence has been printed to enable the continue button
            { 
                continueButton.SetActive(true);
            }
        }
    }
    
    public void NextSentence() //this function, if able, is used to increment the index which in turn moves the dialogue to the next sentence
    {
        Debug. Log ("Inside NextSentence");
        continueButton.SetActive(false); //disables the continue button to avoid bugs if (index ‹ dialogueSentences. Length - 1)
        if (index< dialogueSentences.Length -1) //if there are more sentences then
        {
            index++; //move to the next sentence
            textDisplay.text = ""; //clear the displayed text
            StartCoroutine(TypeDialogue()); //start the coroutine again to display the new sentence
            }
            else
            {
                 //this section gets executed when all sentences have been displayed
                 textDisplay.text = ""; //clear the displayed text
                //continueButton.SetActive(false); //disable the continue button 
                dialogueBox.SetActive(false); //disable the dialogue box
                this.dialogueSentences = null; //clear the sentences array
                index = 0; //reset the index
                playerRB.constraints = RigidbodyConstraints2D.None; //unfreeze the player
                playerRB. constraints = RigidbodyConstraints2D. FreezeRotation; //freeze the player's rotation as it was before
                }
    }


}