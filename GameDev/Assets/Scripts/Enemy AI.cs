using UnityEngine;
using UnityEngine.UIElements;
// This script is for the patrolling enemy AI in the game.
public class EnemyAI : MonoBehaviour
{

    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentTarget;
    public float Speed = 2f;
    public float GiveDistance = 0.5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentTarget = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentTarget.position - transform.position;
        if (currentTarget == pointB.transform)
        {
            rb.linearVelocity = new Vector2(Speed, 0);
            Vector3 localScale = transform.localScale;

            if (localScale.x > 0)
            {
                localScale.x *= -1;
                transform.localScale = localScale;
            }
            
        }
        else if (currentTarget == pointA.transform)
        {
            rb.linearVelocity = new Vector2(-Speed, 0);

            Vector3 localScale = transform.localScale;
            if (localScale.x < 0)
            {
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }

        if (Vector2.Distance(transform.position, currentTarget.position) < GiveDistance && currentTarget == pointA.transform)
        {
            currentTarget = pointB.transform;

        }
        else if (Vector2.Distance(transform.position, currentTarget.position) < GiveDistance && currentTarget == pointB.transform)
        {
            currentTarget = pointA.transform;

        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        Gizmos.DrawWireSphere(pointA.transform.position, GiveDistance);
        Gizmos.DrawWireSphere(pointB.transform.position, GiveDistance);
    }
}

