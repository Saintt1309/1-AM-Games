using UnityEngine;

public class Dashhh : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashCooldown = 1f;
    
    private Rigidbody2D rb;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private bool isDashing;
    private Vector2 dashDirection;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Handle cooldown timer
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        
        // Check for dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            // Get direction based on player's scale
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            dashDirection = new Vector2(direction, 0).normalized;
            
            // Start dash
            isDashing = true;
            dashTimeLeft = dashDuration;
            dashCooldownTimer = dashCooldown;
            
            Debug.Log("Dash started in direction: " + dashDirection);
        }
        
        // Handle dash duration
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                Debug.Log("Dash ended");
            }
        }
    }
    
    void FixedUpdate()
    {
        if (isDashing)
        {
            // Apply dash force
            rb.linearVelocity = dashDirection * dashSpeed;
            Debug.Log("Applying dash velocity: " + rb.linearVelocity);
        }
    }
    
    public bool IsDashing()
    {
        return isDashing;
    }
}