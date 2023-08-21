using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float ballForce;
    [SerializeField] private TextMeshProUGUI ballCountText;
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject directionLine;

    private Vector2 moveVector;
    private Vector2 aimVector;
    private LineRenderer lineRenderer;
    private bool shoot;
    private int ballCount;
    private float shootTimer = 0f;

    public int BallCount
    {
        get
        {
            return ballCount;
        }
        set
        {
            ballCount = value;
        }
    }

    public void InputActionHandler(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            moveVector = context.ReadValue<Vector2>();
        }
        else if (context.action.name == "Shoot")
        {
            shoot = context.ReadValue<float>() > 0.5f;
        }
        else if (context.action.name == "Start" && context.ReadValue<float>() > 0.5f)
        {
            LevelManager.Instance.Begin.SetActive(false);
            LevelManager.Instance.LoadLevel(LevelManager.Instance.LevelIndex);
            directionLine.gameObject.SetActive(true);
            lineRenderer = directionLine.GetComponent<LineRenderer>();
            context.action.Disable();
        }
        else if (context.action.name == "Aim")
        {
            if (context.control.device is Mouse && mainCam != null)
            {
                Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
                aimVector = (mousePosition - (Vector2)transform.position).normalized;
            }
        }
    }

    public void SetPaddle(int ballCount)
    {
        this.ballCount = 0;
        shoot = false;
        shootTimer = 0f;
        this.ballCount = ballCount;
    }

    private void Update()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, (Vector2)transform.position + aimVector * 2f);
        }

        if (shoot)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootBall();
                shootTimer = 0.2f; // Set the interval between shots
            }
        }

        if (ballCount <= 0)
        {
            directionLine.gameObject.SetActive(false);
            ballCountText.text = LevelManager.Instance.BallsReturned.ToString();
            ballCountText.color = Color.black;
        }
        else
        {
            directionLine.gameObject.SetActive(true);
            ballCountText.text = ballCount.ToString();
            ballCountText.color = Color.yellow;
        }
    }

    private void ShootBall()
    {
        if (ballCount <= 0)
        {
            shoot = false;
            return;
        }

        GameObject ball = Instantiate(ballPrefab, (Vector2)transform.position + new Vector2(0, 1), Quaternion.identity);
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();

        Vector2 forceDirection = aimVector;

        ballRb.AddForce(forceDirection * ballForce, ForceMode2D.Impulse);
        ballCount--;
        LevelManager.Instance.AddBall(ball.GetComponent<Ball>());
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
    }
}
