using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float minVelocity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float absorptionDistance;
    [SerializeField] private float rollForce;

    private bool isGrounded = false;
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
    }
    public Vector2 paddlePosition;
    private void FixedUpdate()
    {
        if (isGrounded)
        {
            Vector2 direction = (paddlePosition - rb.position).normalized;

            rb.AddForce(direction * rollForce * Time.deltaTime);
        }
        if (rb.velocity.y < minVelocity || rb.velocity.x < minVelocity)
        {
            rb.AddForce(Vector2.down * rollForce * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            if (Vector2.Distance(rb.position, paddlePosition) < absorptionDistance && isGrounded)
            {
                Destroy(gameObject);
                LevelManager.Instance.RemoveBall(this);
                LevelManager.Instance.CheckReturnedBalls();
            }
        }

        else if (collision.gameObject.CompareTag("Brick"))
        {
            Brick brick = collision.gameObject.GetComponent<Brick>();
            LevelManager.Instance.AddScore(200 * brick.ColorIndex);
            brick.ColorIndex--;
            if (brick.ColorIndex < 0)
            {
                BrickManager.Instance.RemoveBrick(brick);
                BrickManager.Instance.CheckBricks();
            }
            else
            {
                brick.SetSelfColor();
            }
        }
    }
}
