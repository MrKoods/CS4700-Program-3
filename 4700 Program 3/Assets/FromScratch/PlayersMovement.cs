using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	public Rigidbody2D rb;
	[Header("Movement")]
	public float moveSpeed = 5f;
	public Animator animator;
	float horizontalMovement;

	[Header("Jumping")]
	public float jumpPower = 10f;

	[Header("GroundCheck")]
	public Transform groundCheckPosition;
	public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
	public LayerMask groundLayer;

	[Header("Gravity")]
	public float baseGravity = 2f;
	public float maxFallSpeed = 10f;
	public float fallSpeedMultiplier = 2f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		
	}

	void Update()
	{
		rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);

		animator.SetFloat("magnitude", rb.linearVelocity.magnitude);

		Gravity();
	}

	public void Move(InputAction.CallbackContext context)
	{
		horizontalMovement = context.ReadValue<Vector2>().x;
	}
	private bool isGrounded()
	{
		if(Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer))
		{
			return true;
		}
		return false;
	}
	public void Jump(InputAction.CallbackContext context)
	{
		if (isGrounded())
		{
			if (context.performed)
			{
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
			} else if (context.canceled)
			{
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
			}
		}
		
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
	}
	private void Gravity()
	{
		if(rb.linearVelocity.y < 0)
		{
			rb.gravityScale = baseGravity * fallSpeedMultiplier;
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
		}
		else
		{
			rb.gravityScale = baseGravity;
		}
	}
}
