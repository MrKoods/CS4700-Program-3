using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	public Rigidbody2D rb;
	public float moveSpeed = 5f;
	public Animator animator;

	float horizontalMovement;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		
	}

	void Update()
	{
		rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);

		animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
	}

	public void Move(InputAction.CallbackContext context)
	{
		horizontalMovement = context.ReadValue<Vector2>().x;
	}
}
