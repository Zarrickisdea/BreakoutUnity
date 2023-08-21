using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject paddle;
    [SerializeField] private GameObject begin;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI triesText;
    [SerializeField] private TextMeshProUGUI finalText;
    private List<Ball> balls = new List<Ball>();
    private int score = 0;
    private int currentLevelBalls;
    private int ballsReturned = 0;
    private int levelIndex = 1;
    private int tries = 3;

    public TextMeshProUGUI ScoreText
    {
        get
        {
            return scoreText;
        }
    }

    public TextMeshProUGUI TriesText
    {
        get
        {
            return triesText;
        }
    }

    public TextMeshProUGUI FinalText
    {
        get
        {
            return finalText;
        }
    }

    public GameObject Begin
    {
        get
        {
            return begin;
        }
    }

    public int LevelIndex
    {
        get
        {
            return levelIndex;
        }
        set
        {
            levelIndex = value;
        }
    }

    public int CurrentLevelBalls
    {
        get
        {
            return currentLevelBalls;
        }
    }

    public int BallsReturned
    {
        get
        {
            return ballsReturned;
        }
        set
        {
            ballsReturned = value;
        }
    }

    public int ActiveBalls
    {
        get
        {
            return balls.Count;
        }
    }

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        begin.SetActive(true);
    }

    private void Update()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].paddlePosition = paddle.transform.position;
        }

        scoreText.text = score.ToString();
        triesText.text = tries.ToString();
    }

    public void AddBall(Ball ball)
    {
        balls.Add(ball);
    }

    public void RemoveBall(Ball ball)
    {
        balls.Remove(ball);
    }

    public void CheckReturnedBalls()
    {
        ballsReturned++;
        if (ballsReturned >= currentLevelBalls)
        {
            paddle.GetComponent<Paddle>().BallCount = currentLevelBalls;
            ballsReturned = 0;
            tries--;
            if (tries == 0)
            {
                SceneManager.LoadScene("GameOverScene");
                AudioManager.Instance.PlayBGM(AudioManager.BackgroundSound.Over);
                scoreText.transform.parent.gameObject.SetActive(false);
                triesText.transform.parent.gameObject.SetActive(false);
                finalText.gameObject.SetActive(true);
                finalText.text = "Final Score: " + score.ToString();
            }
            else
            {
                BrickManager.Instance.MoveBricks();
            }
        }
    }

    public void DestroyAllBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i] != null)
            {
                Destroy(balls[i].gameObject);
            }

        }
        balls.Clear();
    }
    public void LoadLevel(int levelIndex)
    {
        DestroyAllBalls();
        ballsReturned = 0;
        tries = 3;
        currentLevelBalls = Mathf.Min(100, levelIndex * 10);
        paddle.GetComponent<Paddle>().SetPaddle(currentLevelBalls);
        BrickManager.Instance.GenerateBricks(levelIndex);
    }

    public void AddScore(int score)
    {
        this.score += score;
    }
}
