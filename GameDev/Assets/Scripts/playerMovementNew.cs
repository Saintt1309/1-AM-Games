using UnityEngine;

public class PlayerMovementNew : MonoBehaviour
{
    [SerializeField] private CollisionCheck collisionCheck;
    [SerializeField] private Rigidbody2D rb;
    private Animator anim;

    [Space]
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [Space]
    [Header("Better Jump Settings")]

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 12f;

    [Space]
    [Header("Facing Direction")]
    public bool isFacingRight;

    [Space]
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    [Space]
    [Header("Shrink Settings")]
    [SerializeField] private Vector3 shrinkScale = new Vector3(0.5f, 0.5f, 1f);
    [SerializeField] private float shrinkDuration = 2f;
    [SerializeField] private float shrinkCooldown = 3f;
    private bool isShrinking = false;
    private float shrinkCooldownTimer = 0f;
    private Vector3 originalScale;

    [Space]
    [Header("Grow Settings")]
    [SerializeField] private Vector3 growScale = new Vector3(2f, 2f, 1f);
    [SerializeField] private float growDuration = 2f;
    [SerializeField] private float growGravityScale = 0.5f;
    [SerializeField] private float growCooldown = 3f;
    private bool isGrowing = false;
    private float growCooldownTimer = 0f;

    private bool isWalking;
    private bool isJumping;
    private float isFalling;


    void Start()
    {
        collisionCheck = GetComponent<CollisionCheck>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isWalking = false;
        isJumping = false;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            StartCoroutine(Dash());
            return;
        }

        if (isDashing) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(moveX, moveY);

        Walk(dir);

        isWalking = dir.x != 0;

        if (Input.GetButtonDown("Jump"))
        {
            if (collisionCheck.onGround)
            {
                Jump(Vector2.up);
                isJumping = true;
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        isFalling = rb.linearVelocity.y;

        // Better Jumping physics
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        updateAnim();

        TurnCheck();

        if (shrinkCooldownTimer > 0)
            shrinkCooldownTimer -= Time.deltaTime;

        if (!isShrinking && Input.GetKeyDown(KeyCode.LeftControl) && shrinkCooldownTimer <= 0)
        {
            StartCoroutine(Shrink());
            return;
        }
        if (isShrinking) return;

        if (growCooldownTimer > 0)
            growCooldownTimer -= Time.deltaTime;

        if (!isGrowing && Input.GetKeyDown(KeyCode.LeftAlt) && growCooldownTimer <= 0)
        {
            StartCoroutine(GrowLarge());
            return;
        }
        // Break the fall while large
        if (isGrowing && Input.GetKeyDown(KeyCode.LeftAlt) && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }

    private void Walk(Vector2 dir)
    {
        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

    }

    private void Jump(Vector2 dir)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity += dir * jumpForce;
    }

    void updateAnim()
    {
        anim.SetBool("walking", isWalking);
        anim.SetBool("jump", isJumping);
        anim.SetFloat("yVelocity", isFalling);
    }

    private void TurnCheck()
{
    float moveX = Input.GetAxis("Horizontal");

    if (moveX > 0 && !isFacingRight)
    {
        turn();
    }
    else if (moveX < 0 && isFacingRight)
    {
        turn();
    }
}

    private void turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    
    }

    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2((isFacingRight ? 1 : -1) * dashSpeed, 0);
        anim.SetTrigger("dash"); // Optional: set up a dash animation trigger
        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }

    private System.Collections.IEnumerator Shrink()
    {
        isShrinking = true;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        // Shrink animation
        while (elapsed < 0.3f) // shrink over 0.3 seconds
        {
            transform.localScale = Vector3.Lerp(startScale, shrinkScale, elapsed / 0.3f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = shrinkScale;
        anim.SetTrigger("shrink"); // Optional: set up a shrink animation trigger
        yield return new WaitForSeconds(shrinkDuration);
        // Grow back animation
        elapsed = 0f;
        while (elapsed < 0.3f)
        {
            transform.localScale = Vector3.Lerp(shrinkScale, originalScale, elapsed / 0.3f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
        isShrinking = false;
        shrinkCooldownTimer = shrinkCooldown;
    }

    private System.Collections.IEnumerator GrowLarge()
    {
        isGrowing = true;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        float originalGravity = rb.gravityScale;
        // Grow animation
        while (elapsed < 0.3f)
        {
            transform.localScale = Vector3.Lerp(startScale, growScale, elapsed / 0.3f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = growScale;
        rb.gravityScale = growGravityScale;
        anim.SetTrigger("grow"); // Optional: set up a grow animation trigger
        yield return new WaitForSeconds(growDuration);
        // Shrink back animation
        elapsed = 0f;
        while (elapsed < 0.3f)
        {
            transform.localScale = Vector3.Lerp(growScale, originalScale, elapsed / 0.3f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
        rb.gravityScale = originalGravity;
        isGrowing = false;
        growCooldownTimer = growCooldown;
    }
    
}