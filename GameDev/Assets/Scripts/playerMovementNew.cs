using UnityEngine;

public class PlayerMovementNew : MonoBehaviour
{
    [SerializeField] private CollisionCheck collisionCheck;
    [SerializeField] private Rigidbody2D rb;

    [Space]
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [Space]
    [Header("Better Jump Settings")]

    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 12f;

    void Start()
    {
        collisionCheck = GetComponent<CollisionCheck>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(moveX, moveY);

        Walk(dir);

        if (Input.GetButtonDown("Jump"))
        {
            if (collisionCheck.onGround)
            {
                Jump(Vector2.up);
            }
        }

        // Better Jumping physics
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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
    
}