using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ───── 싱글톤 ─────
    public static GameManager I;
    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);
        Time.timeScale = 1f;

        // 씬 전환시 UI 다시 연결
        SceneManager.activeSceneChanged += (_, __) =>
        {
            Time.timeScale = 1f;
            AutoWireUI();
            UpdateInGameScoreUI();

            if (SceneManager.GetActiveScene().buildIndex == scoreSceneIndex)
            {
                int last = PlayerPrefs.GetInt("LastScore", 0);
                if (scoreSceneText) scoreSceneText.text = last.ToString();
            }
        };
    }

    // ───── 씬 정보 ─────
    [Header("Scene Index")]
    public int gameplaySceneIndex = 1;
    public int scoreSceneIndex = 2;

    // ───── UI 참조 ─────
    [Header("UI")]
    public TMP_Text inGameScoreText;  // 게임 중 점수 텍스트
    public TMP_Text scoreSceneText;   // 점수판 씬 텍스트

    public string inGameScoreName = "CurrentScore";
    public string scoreTextName = "ScoreText";

    // ───── 상태값 ─────
    public int count = 0;
    bool isPaused = false;
    bool isGameOver = false;

    void Start()
    {
        AutoWireUI();
        UpdateInGameScoreUI();
    }

    void Update()
    {
        int cur = SceneManager.GetActiveScene().buildIndex;

        // 게임 씬: 일시정지/재시작
        if (cur == gameplaySceneIndex)
        {
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Q) ||
                Input.GetKeyDown(KeyCode.Escape))
                TogglePause();

            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                isPaused = false;
                isGameOver = false;
                SceneManager.LoadScene(gameplaySceneIndex);
            }
        }
        // 점수판 씬: Space/R = 게임 다시 시작
        else if (cur == scoreSceneIndex)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                isPaused = false;
                isGameOver = false;
                SceneManager.LoadScene(gameplaySceneIndex);
            }
        }
    }

    // ───── 점수 관리 ─────
    public void CountUp(int add)
    {
        count += add;
        Debug.Log($"[GameManager] Count = {count}");
        UpdateInGameScoreUI();
    }

    // ───── 게임오버 처리 ─────
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        PlayerPrefs.SetInt("LastScore", count);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(scoreSceneIndex);
    }

    // ───── 내부 함수들 ─────
    void TogglePause()
    {
        if (isGameOver) return;
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void AutoWireUI()
    {
        if (!inGameScoreText) {
    var go = GameObject.Find("CurrentScore");
    if (go) inGameScoreText = go.GetComponent<TMP_Text>();
}

        if (!scoreSceneText)
        {
            var go2 = GameObject.Find(scoreTextName);
            if (go2) scoreSceneText = go2.GetComponent<TMP_Text>();
        }
    }

    void UpdateInGameScoreUI()
    {
        if (inGameScoreText)
            inGameScoreText.text = count.ToString();
    }
}