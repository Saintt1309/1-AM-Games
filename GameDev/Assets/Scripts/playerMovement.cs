using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration = 7f;
    [SerializeField] private float deceleration = 7f;
    [SerializeField] private float velocityPower = 0.9f;
    [SerializeField] private float frictionAmount = 0.2f;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 6.2f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float fallGravityMultiplier = 1.2f;
    [SerializeField] private int maxAirJumps = 1;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    
    // Private variables
    private float horizontal;
    private bool isFacingRight = true;
    private int airJumpCount;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;
    private float defaultGravityScale;
    
    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
            
        defaultGravityScale = rb.gravityScale;
    }
    
    private void Update()
    {
        // Input handling
        horizontal = Input.GetAxisRaw("Horizontal");
        
        // Jump input buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        // Jump cut (variable jump height)
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
            isJumping = false;
        }
        
        // Coyote time handling
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            airJumpCount = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        // Jump execution
        if (jumpBufferCounter > 0)
        {
            // Ground jump or coyote time jump
            if (coyoteTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                jumpBufferCounter = 0;
            }
            // Air jump
            else if (airJumpCount < maxAirJumps)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                airJumpCount++;
                jumpBufferCounter = 0;
            }
        }
        
        // Gravity scale adjustment for better jump feel
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = defaultGravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = defaultGravityScale;
        }
        
        // Handle character facing direction
        UpdateFacingDirection();
    }
    
    private void FixedUpdate()
    {
        // Apply movement with better physics
        ApplyMovement();
    }
    
    private void ApplyMovement()
    {
        // Calculate target speed
        float targetSpeed = horizontal * moveSpeed;
        
        // Calculate speed difference
        float speedDifference = targetSpeed - rb.velocity.x;
        
        // Calculate acceleration rate based on direction
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        
        // Calculate movement force
        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velocityPower) * Mathf.Sign(speedDifference);
        
        // Apply movement force
        rb.AddForce(movement * Vector2.right);
        
        // Apply friction when not pressing movement keys
        if (Mathf.Abs(horizontal) < 0.01f && IsGrounded())
        {
            float frictionForce = Mathf.Min(Mathf.Abs(rb.velocity.x), frictionAmount);
            frictionForce *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -frictionForce, ForceMode2D.Impulse);
        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    
    private void UpdateFacingDirection()
    {
        if ((isFacingRight && horizontal < 0) || (!isFacingRight && horizontal > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
