// Classe Enemy3.cs

using UnityEngine;

public class Enemy3 : Enemy
{
    public float flySpeed = 2f; // Velocidade do voo horizontal
    public float heightAmplitude = 2f; // Amplitude da altura do voo (quanto o inimigo sobe/desce)
    public float frequency = 1f; // Frequência da oscilação (mais alto = mais rápido o movimento)
    public float directionChangeInterval = 2f; // Intervalo de tempo para mudar a direção horizontal (2 segundos)

    private float timeElapsed;
    private float directionChangeTimer;
    private bool movingRight = true; // Flag para controlar a direção horizontal

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start(); // Chama o Start da classe base (Enemy)
        timeElapsed = 0f;
        directionChangeTimer = 0f;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update(); // Chama o Update da classe base (Enemy)

        // Atualiza o movimento do inimigo
        MoveInZigZag();
    }

    void MoveInZigZag()
    {
        timeElapsed += Time.deltaTime;
        directionChangeTimer += Time.deltaTime;

        // Altera a direção a cada 'directionChangeInterval' segundos
        if (directionChangeTimer >= directionChangeInterval)
        {
            movingRight = !movingRight; // Inverte a direção horizontal
            directionChangeTimer = 0f; // Reseta o timer para o próximo intervalo
        }

        // Movimento horizontal: alterna entre direita e esquerda com base na direção
        float moveDirection = movingRight ? 1f : -1f;

        // Movimento vertical: movimento em zigue-zague (seno)
        float newPositionY = GetStartPosition().y + Mathf.Sin(timeElapsed * frequency) * heightAmplitude;

        // Atualiza a posição do inimigo
        transform.position = new Vector2(transform.position.x + moveDirection * flySpeed * Time.deltaTime, newPositionY);

        // Debugging: Para ver o movimento
        Debug.Log("Flying at position: " + transform.position);
    }
}
