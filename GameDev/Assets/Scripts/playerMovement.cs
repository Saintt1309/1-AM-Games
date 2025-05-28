using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpForce = 6.2f;
    private bool isFacingRight = true;

    private bool airJump;
    
    // Dash variables
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDash = -10f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetButtonDown("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (isGrounded())
        {
            airJump = true;
        }

        Flip();

        if (Input.GetButtonDown("Jump") && airJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            airJump = false;
        }

        // Dash input (using left shift key)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && Time.time >= lastDash + dashCooldown)
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            HandleDash();
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        lastDash = Time.time;

        // Optional: Add visual effects for dash
        // Instantiate(dashEffect, transform.position, Quaternion.identity);
    }

    private void HandleDash()
    {
        // Apply dash force in the direction the player is facing
        float dashDirection = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);

        dashTimeLeft -= Time.deltaTime;

        if (dashTimeLeft <= 0)
        {
            isDashing = false;
            canDash = true;
            
            // Optional: Reduce velocity after dash ends to prevent sliding
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
