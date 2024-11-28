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
}
