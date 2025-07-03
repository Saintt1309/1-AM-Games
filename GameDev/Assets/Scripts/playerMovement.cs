using UnityEngine;

/*public class PlayerMovement : MonoBehaviour
{
    
    
    private float horizontal;
    [SerializeField]private float speed = 8f;
    [SerializeField]private float jumpForce = 6.2f;
    private bool isFacingRight = true;
    private bool airJump;
    private Dashhh dashComponent;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset;

    private void Awake()
    {
        dashComponent = GetComponent<Dashhh>();
    }

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
        UpdateCameraPosition();
    }

    private void FixedUpdate()
    {
        // Only apply normal movement if not dashing
        if (dashComponent == null || !dashComponent.IsDashing())
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
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
            localScale.x *= -1f; // Changed to multiply by -1 instead of setting to -1
            transform.localScale = localScale;
        }
    }

    // Public method to get facing direction (used by Dashhh script)
    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    private void UpdateCameraPosition()
    {
        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
        }
    }
}
*/