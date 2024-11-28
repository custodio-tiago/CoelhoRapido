// Classe Enemy1.cs

using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1f; // Velocidade lenta
        maxHealth = 1; // Apenas 1 de vida
        moveRange = new Vector2(-1f, 1f); // Pequeno intervalo de movimento
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
