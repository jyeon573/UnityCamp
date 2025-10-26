using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ── Singleton ──────────────────────────────────────────────────────────────
    public static GameManager I;
    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 1f;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    // ── Scene Index (프로젝트에 맞게 인스펙터에서 지정) ─────────────────────────
    [Header("Scene Index")]
    public int gameplaySceneIndex = 1;   // 제리가 뛰는 씬
    public int scoreSceneIndex    = 2;   // 점수판 씬

    // ── UI 참조 (선택). 씬마다 자동 탐색도 해줌 ────────────────────────────────
    [Header("Optional UI (Gameplay)")]
    public TMP_Text inGameScoreText;     // 게임 중 점수 UI (없으면 자동 탐색)
    [Header("Optional UI (Score)")]
    public TMP_Text scoreSceneText;      // 점수판 씬의 텍스트 (없으면 자동 탐색)
    [Header("Auto-Find names (fallback)")]
    public string inGameScoreName = "InGameScoreText"; // 게임 씬에서 찾을 이름
    public string scoreTextName   = "ScoreText";       // 점수 씬에서 찾을 이름

    // ── 상태 ───────────────────────────────────────────────────────────────────
    public int count = 0;      // 이번 판 점수
    bool isPaused = false;

    // ── 입력 ───────────────────────────────────────────────────────────────────
    void Update()
    {
        int cur = SceneManager.GetActiveScene().buildIndex;

        // 게임 씬: Space/Q/ESC = 일시정지 토글, R = 재시작
        if (cur == gameplaySceneIndex)
        {
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Q)     ||
                Input.GetKeyDown(KeyCode.Escape))
                TogglePause();

            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                isPaused = false;
                SceneManager.LoadScene(gameplaySceneIndex);
            }
        }
        // 점수판 씬: Space/R = 다시 게임 시작
        else if (cur == scoreSceneIndex)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                isPaused = false;
                SceneManager.LoadScene(gameplaySceneIndex);
            }
        }
    }

    // ── 씬 전환시 초기화/표시 ───────────────────────────────────────────────────
    void OnSceneChanged(Scene prev, Scene next)
    {
        int idx = next.buildIndex;
        Time.timeScale = 1f;   // 혹시 멈춤 상태였다면 복구
        isPaused = false;

        if (idx == gameplaySceneIndex)
        {
            // 새 판 시작
            count = 0;

            // 게임 씬 점수 텍스트 연결 (참조 없으면 이름으로 찾기)
            if (!inGameScoreText)
                inGameScoreText = GameObject.Find(inGameScoreName)?.GetComponent<TMP_Text>();

            UpdateInGameScoreUI();
        }
        else if (idx == scoreSceneIndex)
        {
            // 점수판 표시
            if (!scoreSceneText)
                scoreSceneText = GameObject.Find(scoreTextName)?.GetComponent<TMP_Text>();

            int last = PlayerPrefs.GetInt("LastScore", 0);
            if (scoreSceneText) scoreSceneText.text = last.ToString();
        }
    }

    // ── 게임 중 점수 증가 (코인 등에서 호출) ───────────────────────────────────
    public void CountUp(int add)
    {
        count += add;
        UpdateInGameScoreUI();
    }

    // ── 게임 오버(플레이어 체력 0 등에서 호출) ────────────────────────────────
    public void GameOver()
    {
        PlayerPrefs.SetInt("LastScore", count);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(scoreSceneIndex);
    }

    // ── 내부 유틸 ───────────────────────────────────────────────────────────────
    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void UpdateInGameScoreUI()
    {
        if (inGameScoreText) inGameScoreText.text = count.ToString();
    }
}