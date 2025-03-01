using UnityEngine;

public class dynamicMovement : MonoBehaviour
{
    public float minSpeed = 3.0f;
    public float maxSpeed = 6.0f;
    public bool isLeft = false;  // whether facing Left (starting from right)

    public float leftStartPosition = -7f;
    public float rightStartPosition = 7f;

    private float direction;
    private bool towardsLeft;
    private SpriteRenderer sr;
    private float speed;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);

        transform.position = new Vector2(isLeft ? rightStartPosition : leftStartPosition, transform.position.y);
        towardsLeft = isLeft;
        sr = GetComponent<SpriteRenderer>();

        direction = towardsLeft ? -1f : 1f;
        sr.flipX = towardsLeft;
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        if ((transform.position.x < leftStartPosition && towardsLeft) || (transform.position.x > rightStartPosition && !towardsLeft))
        {
            changeDirection();
        }
    }

    void changeDirection()
    {
        towardsLeft = !towardsLeft;
        direction = towardsLeft ? -1f : 1f;
        sr.flipX = towardsLeft;
    }

}