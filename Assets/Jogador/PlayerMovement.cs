using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 1.1f; // Distância para o Raycast
    public LayerMask groundLayer; // Camada para verificar o chão
    public GameObject projectilePrefab; // Prefab do projétil
    public Transform projectileSpawnPoint; // Ponto de disparo do projétil
    public float projectileSpeed = 10f; // Velocidade do projétil

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
        CheckGroundStatus(); // Verifica se está no chão
        Shoot(); // Chama o método de disparar
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        
        // Suaviza a desaceleração quando o jogador solta a tecla
        float targetVelocityX = moveInput * speed;
        float smoothVelocityX = Mathf.Lerp(rb.velocity.x, targetVelocityX, Time.deltaTime * 10f);
        rb.velocity = new Vector2(smoothVelocityX, rb.velocity.y);

        // Controla a animação de andar
        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.1f);

        // Inverte o sprite dependendo da direção
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false; // Olhando para a direita
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true; // Olhando para a esquerda
        }
    }

    void Jump()
    {
        // Verifica se o jogador apertou a tecla de pulo e está no chão
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            // Aciona a animação de salto (trigger)
            animator.SetTrigger("Jump");
        }
    }
    
    void CheckGroundStatus()
    {
        // Usando um Raycast para verificar se o jogador está no chão
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down * 0.1f, Vector2.down, groundCheckDistance, groundLayer);
        
        if (hit.collider != null)
        {
            Debug.Log("Grounded: " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("Not grounded");
        }
        
        isGrounded = hit.collider != null;
    }

    void Shoot()
    {
        // Verifica se a tecla CTRL foi pressionada
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (projectilePrefab != null && projectileSpawnPoint != null)
            {
                // Instancia o projétil no ponto de spawn
                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

                // Define a direção do projétil
                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                if (projectileRb != null)
                {
                    float direction = spriteRenderer.flipX ? -1f : 1f; // Direção baseada no flip do sprite
                    projectileRb.velocity = new Vector2(direction * projectileSpeed, 0f);
                }
            }
            else
            {
                Debug.LogWarning("Prefab ou ponto de spawn do projétil não está configurado!");
            }
        }
    }
    
    void OnDrawGizmos()
    {
        // Desenha um gizmo para mostrar a distância do Raycast
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        }
    }
}