using UnityEngine;

public class EyeEnemy : MonoBehaviour
{
    [Header("Floating Movement")]
    public float floatHeight = 1.0f;
    public float floatSpeed = 2.0f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        FloatUpAndDown();
    }

    void FloatUpAndDown()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(999); // instant kill
            }
        }
    }
}