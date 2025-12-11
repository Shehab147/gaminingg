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
        
        Application.Quit(); //Exit the game build.
        
        }
    public void onClickRetry()
    {
        SceneManager.LoadScene(4); // Load Level 3 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
