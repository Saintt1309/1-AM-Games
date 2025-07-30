using UnityEngine;
public class CherryController : MonoBehaviour
{
    public AudioClip pickUpSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Destroy the cherry when the player touches it
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickUpSound != null)
            {
                GameObject tempAudio = new GameObject("TempAudio");
                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
                tempSource.clip = pickUpSound;
                tempSource.Play();
                Destroy(tempAudio, pickUpSound.length);
            }
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(pickUpSound.length);
        Destroy(gameObject);
    }
}
