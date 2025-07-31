using UnityEngine;

public class collectemall : MonoBehaviour
{
    private Collider2D col;
    private Renderer rend;
    private bool canBeCollected = false;
    public SceneSwitcher sceneSwitcher;

    void Start()
    {
        // Hide diamond at start
        col = GetComponent<Collider2D>();
        rend = GetComponent<Renderer>();
        if (col != null) col.enabled = false;
        if (rend != null) rend.enabled = false;
    }

    void Update()
    {
        // Check if all cherries are gone
        if (!canBeCollected && CherryController.cherryCount == 0)
        {
            canBeCollected = true;
            if (col != null) col.enabled = true;
            if (rend != null) rend.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canBeCollected && other.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Diamond collected!"); 
            
            sceneSwitcher.LoadEnding();
            Debug.Log("Cherry collected, switching to ending scene.");  

        }
    }
}
