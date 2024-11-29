using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1f;
        maxHealth = 1;
        moveRange = new Vector2(-1f, 1f);
        Debug.Log(name + " configurado com " + maxHealth + " de vida.");
    }
}
