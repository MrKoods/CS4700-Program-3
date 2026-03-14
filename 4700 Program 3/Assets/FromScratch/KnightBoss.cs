using UnityEngine;

public class KnightBoss : MonoBehaviour
{
    public float moveSpeed = 12f; // High speed so he's not slow!
    public Transform leftPoint;
    public Transform rightPoint;
    public int health = 3;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (rb != null) rb.freezeRotation = true;
    }

    void Update()
    {
        if (leftPoint != null && rightPoint != null) Patrol();
    }

    void Patrol()
    {
        float speed = movingRight ? moveSpeed : -moveSpeed;
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        if (movingRight && transform.position.x >= rightPoint.position.x)
        {
            movingRight = false;
            spriteRenderer.flipX = true;
        }
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
        {
            movingRight = true;
            spriteRenderer.flipX = false;
        }
    }

    public void Die()
    {
        anim.SetTrigger("Die");
        this.enabled = false;
        rb.linearVelocity = Vector2.zero;
    }
}