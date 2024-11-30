using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Para usar o componente Text do UI

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
    public int maxLife = 5; // Vida máxima do jogador
    public GameObject stageCompletedTextPrefab; // Prefab do texto "Stage Completed"
    private GameObject stageCompletedTextInstance; // Instância do texto

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int currentLife; // Vida atual do jogador

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLife = maxLife; // Inicializa a vida do jogador
        Debug.Log("Vida inicial do jogador: " + currentLife);

        // Garante que o texto STAGE COMPLETED não esteja visível no início
        if (stageCompletedTextPrefab != null)
        {
            stageCompletedTextInstance = Instantiate(stageCompletedTextPrefab);
            stageCompletedTextInstance.SetActive(false); // Inicialmente invisível

            // Aqui, pegamos o componente Text para garantir que o conteúdo seja atualizado
            Text textComponent = stageCompletedTextInstance.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = ""; // Inicializa o texto como vazio
            }
            else
            {
                Debug.LogWarning("Prefab de 'Stage Completed' não possui o componente Text!");
            }
        }
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
            Debug.Log("Jogador pulou!");
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down * 0.1f, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
        Debug.Log("Estado do jogador no chão: " + isGrounded);
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
                    Debug.Log("Projétil disparado!");
                }
            }
            else
            {
                Debug.LogWarning("Prefab ou ponto de spawn do projétil não está configurado!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger detectado com: " + collision.gameObject.name);

        if (collision.CompareTag("FinishFlag"))
        {
            Debug.Log("Trigger detectado com a bandeira de fim de fase!");

            // Pausa o jogo
            Time.timeScale = 0f;

            // Exibe o texto de "Stage Completed"
            if (stageCompletedTextInstance != null)
            {
                stageCompletedTextInstance.SetActive(true); // Torna o texto visível
                // Atualiza o conteúdo do texto para "STAGE COMPLETED"
                Text textComponent = stageCompletedTextInstance.GetComponent<Text>();
                if (textComponent != null)
                {
                    textComponent.text = "STAGE COMPLETED"; // Define o texto como "STAGE COMPLETED"
                    // Garantir que o texto fique visível no centro da tela
                    stageCompletedTextInstance.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    Debug.LogWarning("STAGE COMPLETED!");
                }
                else
                {
                    Debug.LogWarning("Componente Text não encontrado no prefab de StageCompletedText.");
                }
            }
        }

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Trigger detectado com um inimigo!");

            // Verifica se o jogador está caindo
            bool isFalling = rb.velocity.y < 0;
            if (isFalling)
            {
                // Jogador pulou sobre o inimigo
                rb.velocity = new Vector2(rb.velocity.x, bounceForce); // Adiciona o bounce
                Enemy enemy = collision.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(5); // Causa 5 de dano ao inimigo
                    Debug.Log("Jogador pulou sobre o inimigo e causou 5 de dano!");
                }
                else
                {
                    Debug.LogWarning("Objeto com tag 'Enemy' não possui componente Enemy.");
                }
            }
            else
            {
                // Jogador colidiu normalmente com o inimigo
                TakeDamage(1); // Recebe 1 de dano
                Debug.Log("Jogador colidiu com o inimigo e recebeu 1 de dano.");
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentLife -= damage;
        Debug.Log("Jogador recebeu " + damage + " de dano. Vida restante: " + currentLife);

        if (currentLife <= 0)
        {
            Debug.Log("Jogador morreu. Reiniciando o jogo...");
            RestartGame();
        }
    }

    void RestartGame()
    {
        Debug.Log("Reiniciando a cena...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia a cena atual
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
