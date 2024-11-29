using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;
    public float bounceForce = 5f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
        Jump();
        CheckGroundStatus();
        Shoot();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float targetVelocityX = moveInput * speed;
        float smoothVelocityX = Mathf.Lerp(rb.velocity.x, targetVelocityX, Time.deltaTime * 10f);
        rb.velocity = new Vector2(smoothVelocityX, rb.velocity.y);

        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.1f);

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down * 0.1f, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (projectilePrefab != null && projectileSpawnPoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                if (projectileRb != null)
                {
                    float direction = spriteRenderer.flipX ? -1f : 1f;
                    projectileRb.velocity = new Vector2(direction * projectileSpeed, 0f);
                }
            }
            else
            {
                Debug.LogWarning("Prefab ou ponto de spawn do projétil não está configurado!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Verifica se o player está pulando por cima do inimigo
            bool hitFromAbove = collision.contacts[0].point.y > transform.position.y;
            if (hitFromAbove)
            {
                rb.velocity = new Vector2(rb.velocity.x, bounceForce); // Adiciona o bounce
                enemy.TakeDamage(1); // Inimigo perde 1 de vida
                Debug.Log("Pulo detectado sobre o inimigo: " + enemy.name);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        }
    }
}
