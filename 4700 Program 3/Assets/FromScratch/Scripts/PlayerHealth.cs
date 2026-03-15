using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invincibleTime = 1f;

    private int currentHealth;
    private float invincibleTimer;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincibleTimer > 0)
        {
            return;
        }

        currentHealth -= damage;
        invincibleTimer = invincibleTime;

        Debug.Log("Player took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died.");
        gameObject.SetActive(false);
    }
}