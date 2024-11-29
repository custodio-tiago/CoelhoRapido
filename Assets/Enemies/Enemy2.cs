using UnityEngine;

public class Enemy2 : Enemy
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f;
        maxHealth = 2;
        moveRange = new Vector2(-3f, 3f);
        Debug.Log(name + " configurado com " + maxHealth + " de vida.");
    }
}
