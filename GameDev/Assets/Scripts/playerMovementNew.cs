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

    private bool isWalking;

    void Start()
    {
        collisionCheck = GetComponent<CollisionCheck>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isWalking = false;
    }

    void Update()
    {
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
        updateAnim();

        TurnCheck();
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
        anim.SetBool("Walking", isWalking);
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
    
}