using UnityEngine;

public class EyeEnemy : MonoBehaviour
{
    [Header("Floating Movement")]
    public float floatHeight = 1f;
    public float floatSpeed = 2f;

    [Header("Damage")]
    public int touchDamage = 1;

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
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
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
}