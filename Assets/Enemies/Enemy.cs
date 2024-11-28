// Classe Enemy.cs

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 1;
    protected int currentHealth;
    public Vector2 moveRange = new Vector2(-2f, 2f); // Raio de movimento   

    protected Vector2 startPosition; // Alterado para protected
    protected bool movingLeft = true;

    protected virtual void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        float newPositionX = transform.position.x + (movingLeft ? moveSpeed : -moveSpeed) * Time.deltaTime;

        if (newPositionX > startPosition.x + moveRange.y) movingLeft = false;
        else if (newPositionX < startPosition.x + moveRange.x) movingLeft = true;

        transform.position = new Vector2(newPositionX, transform.position.y);

        // Corrigindo a inversão da animação
        if (movingLeft)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Virar para a direita
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Virar para a esquerda
        }
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

    public Vector2 GetStartPosition()
    {
        return startPosition;
    }
}
