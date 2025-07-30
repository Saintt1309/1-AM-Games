using UnityEngine;

public class AudioTrigger
 : MonoBehaviour
{
    public GameObject panel;
    public AudioClip popSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ShowPanel()
    {
        if (panel != null)
            panel.SetActive(true);
        if (popSound != null && audioSource != null)
            audioSource.PlayOneShot(popSound);
    }
}
