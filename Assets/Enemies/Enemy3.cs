using UnityEngine;

public class Enemy3 : Enemy
{
    public float flySpeed = 2f;
    public float heightAmplitude = 2f;
    public float frequency = 1f;
    public float directionChangeInterval = 2f;

    private float timeElapsed;
    private float directionChangeTimer;
    private bool movingRight = true;

    protected override void Start()
    {
        base.Start();
        timeElapsed = 0f;
        directionChangeTimer = 0f;
    }

    protected override void Update()
    {
        base.Update();
        MoveInZigZag();
    }

    void MoveInZigZag()
    {
        timeElapsed += Time.deltaTime;
        directionChangeTimer += Time.deltaTime;

        if (directionChangeTimer >= directionChangeInterval)
        {
            movingRight = !movingRight;
            directionChangeTimer = 0f;
        }

        float moveDirection = movingRight ? 1f : -1f;
        float newPositionY = GetStartPosition().y + Mathf.Sin(timeElapsed * frequency) * heightAmplitude;
        transform.position = new Vector2(transform.position.x + moveDirection * flySpeed * Time.deltaTime, newPositionY);
    }
}
