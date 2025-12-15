using UnityEngine;

public class BossActivator : MonoBehaviour
{

    public SpecterBoss specter;
    //public GameObject specprefab;
    public Transform spawnPoint;


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BossActivator hit: " + other.name);

        if (other.gameObject.tag == "Player")
        {
            //Instantiate(specter, spawnPoint.transform.position, spawnPoint.transform.rotation);
            specter.ActivateBoss(); // show bar and allow damage
            Destroy(gameObject);    // remove activator so it runs once
        }
    }
}
