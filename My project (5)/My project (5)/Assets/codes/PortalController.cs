using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public int nextLevelIndex ; //based on buldsettings ,scene number 

    public bool isActive = true;

    

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            isActive = false;
            SceneManager.LoadScene(2);
        }
    }
}
    