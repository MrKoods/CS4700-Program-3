using UnityEngine;

public class ImpEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float hopForce = 7f;
    public float hopCooldown = 1f;

    [Header("Patrol")]
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Damage")]
    public int touchDamage = 1;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool movingRight = true;
    private float hopTimer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.2f);
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        hopTimer = hopCooldown;
    }

    void Update()
    {
        Patrol();
        Hop();
        UpdateAnimation();
    }

    void Patrol()
    {
        if (leftPoint == null || rightPoint == null)
        {
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

    void Hop()
    {
        hopTimer -= Time.deltaTime;

        if (hopTimer <= 0f && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, hopForce);
            hopTimer = hopCooldown;
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}