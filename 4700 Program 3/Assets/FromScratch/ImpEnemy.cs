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

    [Header("Stomp")]
    public float stompBounceForce = 10f;
    public float stompVelocityThreshold = -0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.2f);
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool movingRight = true;
    private float hopTimer;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hopTimer = hopCooldown;
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        Patrol();
        Hop();
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
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = true;
                }
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

            if (transform.position.x <= leftPoint.position.x)
            {
                movingRight = true;
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = false;
                }
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
        if (isDead)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            bool playerIsAbove = collision.transform.position.y > transform.position.y + 0.2f;
            bool playerIsFalling = false;

            if (playerRb != null)
            {
                playerIsFalling = playerRb.linearVelocity.y <= stompVelocityThreshold;
            }

            if (playerIsAbove && playerIsFalling)
            {
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, stompBounceForce);
                }

                Die();
            }
            else
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(touchDamage);
                }
            }
        }
    }

    void Die()
    {
        isDead = true;

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}