using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    const string ScoreKey = "LastScore";

    public int gameScene = 0;
    public int scoreScene = 1;

    TMP_Text scoreText;
    int score = 0;
    bool gameOver = false;
    bool paused;   // 일시정지 상태

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += (_, __) =>
        {
            Time.timeScale = 1f;
            AutoFind();

            int idx = SceneManager.GetActiveScene().buildIndex;

            if (idx == gameScene)
            {
                StartRun();
            }
            else if (idx == scoreScene)
            {
                int last = PlayerPrefs.GetInt(ScoreKey, 0);
                if (scoreText) scoreText.text = $"{last}";
            }
        };
    }

    void Start()
    {
        AutoFind();
        UpdateUI();
    }

    void AutoFind()
    {
        var go = GameObject.Find("CurrentScore");
        scoreText = go ? go.GetComponent<TMP_Text>() : null;
    }

    public void CountUp(int add)
    {
        score += add;
        Debug.Log($"score = {score}");
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        if (gameOver) return;
        gameOver = true;

        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();

        SceneManager.LoadScene(scoreScene);
    }

    void StartRun()
    {
        score = 0;
        gameOver = false;
        Time.timeScale = 1f;
        AutoFind();
        UpdateUI();
    }

    // ───── 내부 함수들 ─────
    void Update()
{
    int idx = SceneManager.GetActiveScene().buildIndex;

    // 게임 씬에서 일시정지/해제
    if (idx == gameScene)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
        }
    }
    // 점수판 씬에서 R 또는 Space → 재시작
    else if (idx == scoreScene)
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            gameOver = false;
            SceneManager.LoadScene(gameScene);
        }
    }
}
    void StartNewRun()
{
    int idx = SceneManager.GetActiveScene().buildIndex;

    // 점수판 씬에서 R 또는 Space 누르면 다시 시작
    if (idx == scoreScene)
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            gameOver = false;
            SceneManager.LoadScene(gameScene);
        }
    }
}
}

