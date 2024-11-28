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
}
