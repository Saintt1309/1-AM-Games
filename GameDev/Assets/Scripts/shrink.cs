using UnityEngine;
using System.Collections;

public class Shrink : MonoBehaviour
{
    [Header("Shrink Settings")]
    [SerializeField] private float shrinkScale = 0.5f;     // How small the character becomes (0.5 = half size)
    [SerializeField] private float shrinkDuration = 5f;    // How long the shrink lasts
    [SerializeField] private float shrinkCooldown = 8f;    // Cooldown before can shrink again
    [SerializeField] private KeyCode shrinkKey = KeyCode.E; // Key to activate shrink
    [SerializeField] private float transitionSpeed = 5f;   // How fast to shrink/grow
    
    [Header("Optional Effects")]
    [SerializeField] private ParticleSystem shrinkEffect;  // Optional particle effect
    [SerializeField] private AudioClip shrinkSound;        // Optional sound effect
    [SerializeField] private Color shrinkColor = Color.cyan; // Color tint when shrunk
    
    // Private variables
    private Vector3 originalScale;                // Original character scale
    private Vector3 targetScale;                  // Target scale when shrinking
    private bool isShrunk = false;                // Is currently shrunk
    private bool isTransitioning = false;         // Is currently changing size
    private float lastShrinkTime = -10f;          // Time of last shrink
    private SpriteRenderer spriteRenderer;        // Reference to sprite renderer
    private Color originalColor;                  // Original sprite color
    private AudioSource audioSource;              // Reference to audio source
    
    private void Awake()
    {
        // Store original scale
        originalScale = transform.localScale;
        targetScale = originalScale;
        
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && shrinkSound != null)
        {
            // Add audio source if needed
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }
    
    private void Update()
    {
        // Check for shrink input
        if (Input.GetKeyDown(shrinkKey) && !isTransitioning && Time.time >= lastShrinkTime + shrinkCooldown)
        {
            ToggleShrink();
        }
        
        // Smoothly transition between scales
        if (isTransitioning)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
            
            // Check if transition is complete
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale;
                isTransitioning = false;
            }
        }
    }
    
    private void ToggleShrink()
    {
        if (!isShrunk)
        {
            // Start shrinking
            StartShrink();
        }
        else
        {
            // Return to normal size
            StopShrink();
        }
    }
    
    private void StartShrink()
    {
        isShrunk = true;
        isTransitioning = true;
        lastShrinkTime = Time.time;
        
        // Calculate target scale (maintain X direction for facing)
        float xDirection = Mathf.Sign(transform.localScale.x);
        targetScale = new Vector3(
            originalScale.x * shrinkScale * xDirection,
            originalScale.y * shrinkScale,
            originalScale.z
        );
        
        // Play effects
        PlayShrinkEffects(true);
        
        // Start timer to automatically return to normal
        StartCoroutine(ShrinkTimer());
    }
    
    private void StopShrink()
    {
        isShrunk = false;
        isTransitioning = true;
        
        // Calculate target scale (maintain X direction for facing)
        float xDirection = Mathf.Sign(transform.localScale.x);
        targetScale = new Vector3(
            originalScale.x * xDirection,
            originalScale.y,
            originalScale.z
        );
        
        // Play effects
        PlayShrinkEffects(false);
    }
    
    private IEnumerator ShrinkTimer()
    {
        yield return new WaitForSeconds(shrinkDuration);
        
        // Only return to normal if still shrunk
        if (isShrunk)
        {
            StopShrink();
        }
    }
    
    private void PlayShrinkEffects(bool shrinking)
    {
        // Play particle effect
        if (shrinkEffect != null)
        {
            shrinkEffect.Play();
        }
        
        // Play sound effect
        if (audioSource != null && shrinkSound != null)
        {
            audioSource.PlayOneShot(shrinkSound);
        }
        
        // Change color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = shrinking ? shrinkColor : originalColor;
        }
    }
    
    // Public method to check if player is currently shrunk
    public bool IsShrunk()
    {
        return isShrunk;
    }
    
    // Public method to force return to normal size
    public void ResetSize()
    {
        if (isShrunk)
        {
            StopShrink();
        }
    }
}