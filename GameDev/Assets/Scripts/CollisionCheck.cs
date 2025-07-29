using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]
    [Header("Collision States")]
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    public bool isDead = false;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    [Space]
    [Header("Death Screen")]
    public GameObject deathScreen;

    [Space]

    [Header("Spike Collision")]

    public PhysicsMaterial2D spikeMaterial;
    public LayerMask spikeLayer;

    [Space]

    [Header("Enemy Collision")]

    public PhysicsMaterial2D enemyMaterial;
    public LayerMask enemyLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;

        if (isDead)
        {
            deathScreen.SetActive(true);
        }
        else
        {
            deathScreen.SetActive(false);
        }
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & spikeLayer) != 0)
        {
            // The (1 << collision.gameObject.layer) creates a bitmask for the collided object's layer.
            // The & operator checks if any bits overlap with 'spikeLayer'.
            // If they do, it means the collided object is on a layer included in 'spikeLayer'.
            Debug.Log("Hit by spikes (Layer Check)!");
            isDead = true;

        }
        else if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Debug.Log("Hit by enemy (Layer Check)!");
            isDead = true;

        }
        else
        {
            isDead = false;
        }
    }

}
