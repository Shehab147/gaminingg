using UnityEngine;
using UnityEngine.UI;

public class SpecterBoss : MonoBehaviour
{
    public float maxHealth = 100f;
    public Image greenbar;     // assign UI Slider
    public Image yellowbar;     // YellowBar (shows under green)

    public Image redbar; //shows under yellow 

    public float contactDamage = 20f;

    float currentHealth;
    bool isActive = false;

    void Start()
    {
        currentHealth = maxHealth;

        // hide bar until fight starts
        greenbar.gameObject.SetActive(false);
        yellowbar.gameObject.SetActive(false);
        redbar.gameObject.SetActive(false);


        // start full
        greenbar.fillAmount = 1f;
        yellowbar.fillAmount=1f;
        redbar.fillAmount=1f;
    }

    void Update(){
    if (!isActive) return;  // exit early if boss not active


        //show only the appropiate bar based on health
        float healthPercent = currentHealth / maxHealth;

        // Show only the appropriate bar based on health
        if (healthPercent > 0.5f)
        {
            // Above 50%: show only green
            greenbar.gameObject.SetActive(true);
            yellowbar.gameObject.SetActive(false);
            redbar.gameObject.SetActive(false);
            
            greenbar.fillAmount = healthPercent;
        }
        else if (healthPercent > 0.1f)
        {
            // Between 10% and 50%: show only yellow
            greenbar.gameObject.SetActive(false);
            yellowbar.gameObject.SetActive(true);
            redbar.gameObject.SetActive(false);
            
            yellowbar.fillAmount = healthPercent;
        }
        else
        {
            // Below 10% (one shot left): show only red
            greenbar.gameObject.SetActive(false);
            yellowbar.gameObject.SetActive(false);
            redbar.gameObject.SetActive(true);
            
            redbar.fillAmount = healthPercent;
        }
    }
        

    public void ActivateBoss()
    {
        Debug.Log("Boss activated");
        isActive = true;
        //start with green bar visible 
        greenbar.gameObject.SetActive(true);
        
    }

    public void TakeDamage(float amount)
    {
        if (!isActive) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
            redbar.gameObject.SetActive(false);

        }
    }

    void Die()
    {
        // play death animation, end game, etc.
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(contactDamage);
        }
    }
}
