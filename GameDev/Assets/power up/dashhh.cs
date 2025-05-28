using UnityEngine;

public class Dashhh : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private TrailRenderer dashTrail; // Optional for visual effect

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    private PlayerMovement playerMovement; // Reference to the player movement script

    // Dash state variables
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime = -10f;
    private Vector2 dashDirection;

    private void Awake()
    {
        // Auto-reference components if not assigned
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        
        // If trail renderer is assigned, disable it initially
        if (dashTrail != null) dashTrail.emitting = false;
    }

    private void Update()
    {
        // Check for dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }

        // Update dash state
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            
            if (dashTimeLeft <= 0)
            {
                StopDash();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            // Apply dash velocity
            rb.linearVelocity = dashDirection * dashSpeed;
        }
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        // Use the PlayerMovement's facing direction
        if (playerMovement != null)
        {
            float facingDirection = playerMovement.IsFacingRight() ? 1f : -1f;
            dashDirection = new Vector2(facingDirection, 0f).normalized;
        }
        else
        {
            // Fallback if playerMovement reference is missing
            float facingDirection = transform.localScale.x > 0 ? 1f : -1f;
            dashDirection = new Vector2(facingDirection, 0f).normalized;
        }

        // Enable trail effect if available
        if (dashTrail != null) dashTrail.emitting = true;

        // Optional: Play dash sound effect
        // AudioManager.Instance.PlaySound("DashSound");
    }

    private void StopDash()
    {
        isDashing = false;
        canDash = true;
        
        // Reduce velocity after dash to prevent sliding
        rb.linearVelocity = rb.linearVelocity * 0.5f;

        // Disable trail effect
        if (dashTrail != null) dashTrail.emitting = false;
    }

    // Public method to check if player is currently dashing (can be used by other scripts)
    public bool IsDashing()
    {
        return isDashing;
    }
}