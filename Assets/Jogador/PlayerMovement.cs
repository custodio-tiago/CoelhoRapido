﻿using UnityEngine;
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
    public GameObject heartPrefab; // Prefab de coração para UI
    public Transform lifePanel;   // Painel para os corações de vida
    public int maxCoins = 100; // Máximo de moedas que o jogador pode coletar
    public Text coinText; // UI para exibir o número de moedas

    private GameObject stageCompletedTextInstance; // Instância do texto
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int currentLife; // Vida atual do jogador
    private int currentCoins; // Moedas coletadas

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentLife = maxLife; // Inicializa a vida do jogador
        currentCoins = 0; // Inicializa as moedas do jogador
        Debug.Log("Vida inicial do jogador: " + currentLife);
        Debug.Log("Moedas iniciais: " + currentCoins);

        // Atualiza a UI com os corações iniciais
        UpdateLifeUI();
        UpdateCoinUI();

        // Encontrar o objeto de texto com a tag "FinishText"
        GameObject stageCompletedTextObject = GameObject.FindGameObjectWithTag("FinishText");
        if (stageCompletedTextObject != null)
        {
            stageCompletedTextInstance = stageCompletedTextObject;
            stageCompletedTextInstance.SetActive(false); // Inicialmente invisível

            Text textComponent = stageCompletedTextInstance.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = ""; // Inicializa o texto como vazio
            }
            else
            {
                Debug.LogWarning("Objeto com tag 'FinishText' não possui o componente Text!");
            }
        }
        else
        {
            Debug.LogWarning("Nenhum objeto encontrado com a tag 'FinishText'!");
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
                Text textComponent = stageCompletedTextInstance.GetComponent<Text>();
                if (textComponent != null)
                {
                    textComponent.text = "STAGE COMPLETED"; // Define o texto como "STAGE COMPLETED"
                    stageCompletedTextInstance.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    Debug.LogWarning("STAGE COMPLETED!");
                }
                else
                {
                    Debug.LogWarning("Componente Text não encontrado no objeto de 'FinishText'.");
                }
            }
        }

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Trigger detectado com um inimigo!");

            bool isFalling = rb.velocity.y < 0;
            if (isFalling)
            {
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
                TakeDamage(1); // Recebe 1 de dano
                Debug.Log("Jogador colidiu com o inimigo e recebeu 1 de dano.");
            }
        }

        if (collision.CompareTag("Coin"))
        {
            Debug.Log("Trigger detectado com uma moeda!");
            CollectCoin(1); // Coleta 1 moeda
            Destroy(collision.gameObject); // Destroi a moeda após coletá-la
        }

        if (collision.CompareTag("Heart"))
        {
            Debug.Log("Trigger detectado com um coração!");
            CollectHeart(1); // Coleta 1 coração
            Destroy(collision.gameObject); // Destroi o coração após coletá-lo
        }
    }

    void TakeDamage(int damage)
    {
        currentLife -= damage;
        Debug.Log("Jogador recebeu " + damage + " de dano. Vida restante: " + currentLife);

        // Atualiza a UI
        UpdateLifeUI();

        if (currentLife <= 0)
        {
            Debug.Log("Jogador morreu. Reiniciando o jogo...");
            RestartGame();
        }
    }

    void CollectCoin(int amount)
    {
        currentCoins += amount;
        currentCoins = Mathf.Clamp(currentCoins, 0, maxCoins); // Garante que não ultrapasse o máximo
        Debug.Log("Jogador coletou " + amount + " moeda(s). Moedas totais: " + currentCoins);

        // Atualiza a UI de moedas
        UpdateCoinUI();
    }

    void CollectHeart(int amount)
    {
        currentLife += amount;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife); // Garante que não ultrapasse o máximo
        Debug.Log("Jogador coletou " + amount + " coração(ões). Vida atual: " + currentLife);

        // Atualiza a UI de vida
        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        // Limpa os corações existentes
        foreach (Transform child in lifePanel)
        {
            Destroy(child.gameObject);
        }

        // Instancia novos corações de acordo com a vida atual
        for (int i = 0; i < currentLife; i++)
        {
            Instantiate(heartPrefab, lifePanel);
        }

        // Atualiza o tamanho do LifePanel dinamicamente
        RectTransform rectTransform = lifePanel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float newWidth = 40f * currentLife; // Cada coração ocupa 40 de largura
            rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
        }
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "" + currentCoins; // Atualiza o texto da UI com as moedas coletadas
            Debug.Log(currentCoins);
        }
    }

    void RestartGame()
    {
        Debug.Log("Reiniciando a cena...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia a cena atual
    }

    void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
    }
}
