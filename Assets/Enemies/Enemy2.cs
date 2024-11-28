// Classe Enemy2.cs

using UnityEngine;

public class Enemy2 : Enemy
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f; // Velocidade normal
        maxHealth = 2; // Precisa de 2 hits para morrer
        moveRange = new Vector2(-3f, 3f); // Maior intervalo de movimento
    }

    protected override void Move()
    {
        float newPositionX = transform.position.x + (movingLeft ? moveSpeed : -moveSpeed) * Time.deltaTime;

        if (newPositionX > startPosition.x + moveRange.y) movingLeft = false;
        else if (newPositionX < startPosition.x + moveRange.x) movingLeft = true;

        transform.position = new Vector2(newPositionX, transform.position.y);

        // Corrigindo a inversão da animação, tem que ficar invertido aqui pra funcionar mesmo
        if (movingLeft)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Virar para a esquerda
        }
        else
        {           
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Virar para a direita
        }
    }
}
