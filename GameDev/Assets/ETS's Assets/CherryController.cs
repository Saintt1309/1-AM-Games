using UnityEngine;

public class CherryController
 : MonoBehaviour
{
    // Destroy the cherry when the player touches it
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
