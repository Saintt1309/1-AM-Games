using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip deathSound;
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip dashSound;
    public AudioClip shrinkSound;
    public AudioClip growSound;
    public AudioClip respawnSound;



    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (MusicSource != null && backgroundMusic != null)
        {
            MusicSource.clip = backgroundMusic;
            MusicSource.loop = true;
            MusicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    public void PlayDeathSound() { PlaySFX(deathSound); }
    public void PlayJumpSound() { PlaySFX(jumpSound); }
    public void PlayLandingSound() { PlaySFX(landingSound); }
    public void PlayDashSound() { PlaySFX(dashSound); }
    public void PlayShrinkSound() { PlaySFX(shrinkSound); }
    public void PlayGrowSound() { PlaySFX(growSound); }
    public void PlayRespawnSound() { PlaySFX(respawnSound); }
}