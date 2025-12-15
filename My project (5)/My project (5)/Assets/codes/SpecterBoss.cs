using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SpecterBoss : MonoBehaviour
{
    public float maxHealth = 100f; 
    public Image greenbar;     // assign UI Slider
    public Image yellowbar;     // YellowBar (shows under green)

    public Image redbar; //shows under yellow 

    public float contactDamage = 20f;

    // follow settings
    public Transform player;      
    public float moveSpeed = 3f;  // how fast specter moves
    public float stopDistance = 1.5f; // how close it gets to player
    public Rigidbody2D rb; // used so movement is according to physics 
    private SpriteRenderer sr; // used for flipping 

    float currentHealth;
    bool isActive = false;

    void Start()
    {
        currentHealth = maxHealth; 

         rb = GetComponent<Rigidbody2D>(); //gets rigidbody from object which is specter

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
    if (!isActive) return;  // skip if boss not active


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
        SceneManager.LoadScene(7);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(contactDamage);
        }
    }
    void FixedUpdate()
{
    if (!isActive) return; //dont move if boss isnt activated
    if (player == null) return; //if no target is assigned ,it doesnt move 
    // FLIP based on player's X direction (NOT Y)
    // This stays correct even while the player jumps. [file:8]
       
        if (sr != null)
        {
            // If player is to the RIGHT of boss, face right; else face left.
            
            if (player.position.x > transform.position.x)
                sr.flipX = false;
            else
                sr.flipX = true;
        }
        //follow 

    float dist = Vector2.Distance(rb.position, player.position); ////measure distance to player
    
    if (dist > stopDistance)// if player is close enough ,stop horizontal movement 
    {
        if (rb != null)
        rb.velocity = new Vector2(0f, rb.velocity.y);
        return;
    }// Calculate the next position toward the player
        Vector2 newPos = Vector2.MoveTowards(transform.position,player.position,moveSpeed * Time.fixedDeltaTime);  // FixedUpdate uses fixedDeltaTime

        // Move the Rigidbody if present (more physics-friendly than transform-only movement)
        if (rb != null)
            rb.MovePosition(newPos);
        else
            transform.position = newPos; // fallback if no Rigidbody2D exists
    }

}
