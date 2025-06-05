using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject ball;
    public GameObject scoreboard;
    public Text scoreTextLeft;
    public Text scoreTextRight;
    public bool started = false;
    private int scoreLeft = 0;
    private int scoreRight = 0;
    private BallController ballController;
    private Vector3 startingPosition;
    
    public float gameDuration = 20f;
    private float elapsedTime = 0f;
    public int maxScore = 5;
    public Text timerText;
    private Vector3 originalTimerScale;

    private Color defaultTimerColor;
    public Color flashColor = new Color(1f, 0.3f, 0.3f);


    public GameObject winPanel;
    public GameObject timeoutTitle;
    public Text winText;
    public Text winTitleText;
    public GameObject victoryFireworksLeft;
    public GameObject victoryFireworksRight;
    public AudioSource fireworksAudio;

    private bool isMenuMode = false;
    public bool gameEnded = false;

    public enum AIDifficulty { Easy, Medium, Hard }
    public AIDifficulty currentDifficulty = AIDifficulty.Medium; // default

    public bool isGuideMode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ballController = this.ball.GetComponent<BallController>();
        this.startingPosition = Vector3.zero;
        timerText.gameObject.SetActive(false);
        scoreboard.SetActive(false);
        defaultTimerColor = timerText.color;
        originalTimerScale = timerText.rectTransform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        if (!started || gameEnded)
            return;

        elapsedTime += Time.deltaTime;
        UpdateTimerUI();

        if (!isGuideMode && elapsedTime >= gameDuration)
        {
            EndGame("Time's Up!");
        }    
    }


    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // ליתר ביטחון
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(bool isVsAI, string difficulty)
    {
        isMenuMode = false;
        this.started = true;
        scoreboard.SetActive(true);
        timerText.gameObject.SetActive(true);

        // Reset scores
        scoreLeft = 0;
        scoreRight = 0;
        UpdateUI();
        UpdateTimerUI();
        ResetBall();

        ballController.Go();

        if (isVsAI)
        {
            Debug.Log("Starting game vs AI");
            foreach (RacketController racket in FindObjectsOfType<RacketController>())
            {
                // Enable AI for one racket
                if (racket.gameObject.name != "PlayerTwo")
                {
                    racket.enabled = true;
                    racket.isPlayer = true;
                }
                else
                {
                    racket.enabled = true;
                    racket.isPlayer = false;
                    // Set AI difficulty
                    switch (difficulty)
                    {
                        case "easy":
                            currentDifficulty = AIDifficulty.Easy;
                            racket.baseSpeed = 6.5f;
                            ballController.speed = 10f;
                            break;
                        case "medium":
                            currentDifficulty = AIDifficulty.Medium;
                            racket.baseSpeed = 15f;
                            ballController.speed = 15f;
                            break;
                        case "hard":
                            currentDifficulty = AIDifficulty.Hard;
                            racket.baseSpeed = 25f;
                            ballController.speed = 25f;
                            break;
                        default:
                            currentDifficulty = AIDifficulty.Medium;
                            break;
                    }
                    racket.speed = racket.baseSpeed;
                }

            }
        }
        else
        {
            Debug.Log("Starting game vs Player");
            foreach (RacketController racket in FindObjectsOfType<RacketController>())
            {
                racket.enabled = true;
                racket.isPlayer = true;
            }
        }
    }

    public void ScoreGoalLeft()
    {
        if (isMenuMode)
        {
            ResetBall(); // just reset and ignore score
            return;
        }

        this.scoreRight++;
        UpdateUI();
        ResetBall();
        CheckForGameEnd();
        if (!gameEnded) ResetBall();
    }

    public void ScoreGoalRight()
    {
        if (isMenuMode)
        {
            ResetBall();
            return;
        }

        this.scoreLeft++;
        UpdateUI();
        ResetBall();
        CheckForGameEnd();
        if (!gameEnded) ResetBall();
    }

    private void UpdateUI()
    {
        this.scoreTextLeft.text = this.scoreLeft.ToString();
        this.scoreTextRight.text = this.scoreRight.ToString();
    }

    private void UpdateTimerUI()
{
    float remainingTime = Mathf.Max(0f, gameDuration - elapsedTime);
    int minutes = Mathf.FloorToInt(remainingTime / 60f);
    int seconds = Mathf.FloorToInt(remainingTime % 60f);

    timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    if (remainingTime <= 30f)
    {
        // הבהוב צבע
        float t = Mathf.PingPong(Time.time * 4f, 1f);
        timerText.color = Color.Lerp(defaultTimerColor, flashColor, t);

        // פולס בקנה מידה (scale bounce)
        float pulse = 1f + Mathf.Sin(Time.time * 6f) * 0.05f;
        timerText.rectTransform.localScale = originalTimerScale * pulse;
    }
    else
    {
        timerText.color = defaultTimerColor;
        timerText.rectTransform.localScale = originalTimerScale;
    }
}


    private void ResetBall()
    {
        this.ballController.Stop();
        this.ball.transform.position = this.startingPosition;
        this.ballController.Go();
        foreach (RacketController racket in FindObjectsByType<RacketController>(FindObjectsSortMode.None))
        {
            racket.ResetSpeed();
        }
    }

    private void CheckForGameEnd()
    {
        if (isGuideMode)
            return;  // Ignore ending the game while in Guide Mode

        if (scoreLeft >= maxScore)
        {
            EndGame("Left Player");
        }
        else if (scoreRight >= maxScore)
        {
            EndGame("Right Player");
        }
    }

    private void EndGame(string message)
    {
        victoryFireworksLeft.SetActive(false);
        victoryFireworksRight.SetActive(false);
        // if (backgroundMusic != null)
        // {
        //     backgroundMusic.volume = 0.2f;
        // }


        timeoutTitle.SetActive(false);
        timerText.gameObject.SetActive(false);
        gameEnded = true;
        started = false;

        ballController.Stop();
        winPanel.SetActive(true);
        if (fireworksAudio != null)
        {
            fireworksAudio.Play();
        }
        winText.text = message;

        if ( message == "Time's Up!")
        {
            if (scoreLeft > scoreRight)
            {
                winText.text = "Left Player";
                victoryFireworksLeft.SetActive(true);

            }
            else if (scoreRight > scoreLeft)
            {
                winText.text = "Right Player";
                victoryFireworksRight.SetActive(true);

            }
            else
            {
                winText.text = "It's a Tie!";
                winTitleText.text = "and...";
            }
            timeoutTitle.SetActive(true);
        }
        else if (message == "Left Player")
        {
            victoryFireworksLeft.SetActive(true);
        }
        else if (message == "Right Player")
        {
            victoryFireworksRight.SetActive(true);
        }


        foreach (RacketController racket in FindObjectsOfType<RacketController>())
        {
            racket.enabled = false;
        }
    }   

    public void startOnlyAiGame()
    {
        isMenuMode = true;
        gameEnded = false;
        started = false;

        ballController.Go();
        foreach (RacketController racket in FindObjectsOfType<RacketController>())
            {
                racket.enabled = true;
                racket.isPlayer = false;
            }
    }

    public void enableAllRackets()
    {
        foreach (RacketController racket in FindObjectsOfType<RacketController>())
        {
            racket.enabled = true;
            racket.isPlayer = true;
        }
    }
}

