using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 1;
    protected int currentHealth;
    public Vector2 moveRange = new Vector2(-2f, 2f); // Raio de movimento
    public LayerMask groundLayer;

    private Vector2 startPosition;
    private bool movingRight = true;

    protected virtual void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        Move();
    }

    protected void Move()
    {
        float newPositionX = transform.position.x + (movingRight ? moveSpeed : -moveSpeed) * Time.deltaTime;

        if (newPositionX > startPosition.x + moveRange.y) movingRight = false;
        else if (newPositionX < startPosition.x + moveRange.x) movingRight = true;

        transform.position = new Vector2(newPositionX, transform.position.y);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
