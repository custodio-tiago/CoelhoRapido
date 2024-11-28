using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    private float direction;

    public void SetDirection(float dir)
    {
        direction = dir;
    }

    void Update()
    {
        // Move o projétil na direção indicada
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject); // Destroi o projétil após atingir o inimigo
            }
        }
        else if (collision.CompareTag("Plataforma"))
        {
            Destroy(gameObject); // Destroi o projétil ao tocar o chão
        }
    }
}
