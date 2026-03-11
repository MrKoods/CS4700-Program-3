using UnityEngine;

public class GolemEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;

    [Header("Patrol")]
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Damage")]
    public int touchDamage = 2;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool movingRight = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.2f);
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Patrol();
        UpdateAnimation();
    }

    void Patrol()
    {
        if (leftPoint == null || rightPoint == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (movingRight)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

            if (transform.position.x >= rightPoint.position.x)
            {
                movingRight = false;
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

            if (transform.position.x <= leftPoint.position.x)
            {
                movingRight = true;
                spriteRenderer.flipX = false;
            }
        }
    }

    bool IsGrounded()
    {
        if (groundCheck == null)
        {
            return false;
        }

        Collider2D hit = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        return hit != null;
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
            animator.SetBool("grounded", IsGrounded());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}