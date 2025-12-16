using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NavigationController : MonoBehaviour
{
    public void onClickStart(){

        SceneManager.LoadScene(1);//Load Level 1
    }
    public void onClickQuit(){
        
        Debug.Log("Quit clicked");

        Application.Quit(); //Exit the game build.
        
        }
    public void onClickRetry()
    {
        SceneManager.LoadScene(5); // Load Level 3 
    }
    public void onClickRestartLevel()
    {
    SceneManager.LoadScene(1);
    }
    
}
