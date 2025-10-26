using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI (선택)")]
    public TMP_Text inGameScoreText;   // 진행 중 점수 표시 텍스트(없으면 비워둬도 됨)

    [Header("Scene Index (원하는 인덱스로 맞춰줘)")]
    public int gameplaySceneIndex = 1; // 게임 씬
    public int scoreSceneIndex    = 2; // 점수판 씬

    // 상태
    public int count = 0;     // 이번 판 점수
    bool isPaused = false;
    bool isGameOver = false;

    void Awake()
    {
        // 혹시 이전 씬에서 멈춰있던 거 복구
        Time.timeScale = 1f;
        count = 0;
        UpdateScoreUI();
    }

    void Update()
    {
        // 스페이스 / Q / ESC = 일시정지 토글 (게임 중에만)
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Q)     ||
            Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // R = 현재 게임 재시작
        if (Input.GetKeyDown(KeyCode.R))
        {
            isGameOver = false;
            isPaused   = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // 점수 올리기(코인에서 이거 호출)
    public void CountUp(int add)
    {
        if (isGameOver) return;
        count += add;
        UpdateScoreUI();
    }

    // 게임 오버 때 호출(플레이어 체력 0 등)
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 점수 저장 → 점수판 씬에서 읽기
        PlayerPrefs.SetInt("LastScore", count);
        PlayerPrefs.Save();

        // 점수판으로 이동 (다음 씬에서 입력 먹게 1로)
        Time.timeScale = 1f;
        SceneManager.LoadScene(scoreSceneIndex);
    }

    // ===== 내부 =====
    void TogglePause()
    {
        if (isGameOver) return; // 게임 오버 화면에선 의미 없음
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void UpdateScoreUI()
    {
        if (inGameScoreText) inGameScoreText.text = count.ToString();
    }
}