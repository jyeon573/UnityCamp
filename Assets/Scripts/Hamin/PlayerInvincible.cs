using System.Collections;
using UnityEngine;

public class PlayerInvincible : MonoBehaviour
{
    [Header("Invincible")]
    public float invincibilityDuration = 2f; // 무적 지속
    public float blinkInterval = 0.2f;       // 깜빡임 간격
    public bool isInvincible = false;

    Renderer[] renderers;
    Coroutine invincibilityCoroutine;

    [Header("UI & Refs")]
    public DamageOverlay damageOverlay;
    public UnityEngine.UI.Image heart3;
    public UnityEngine.UI.Image heart2;
    public UnityEngine.UI.Image heart1;
    public Sprite binHeart;

    [Header("State")]
    public int heart = 3;

    GameManager gm;  // GameManager 인스턴스

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            Debug.LogError("No Renderer found on player or its children!");

        // 인스펙터에 안 꽂았으면 자동으로 찾아 사용
        if (!damageOverlay) damageOverlay = GetComponent<DamageOverlay>();
        gm = FindObjectOfType<GameManager>();
        if (!gm) Debug.LogError("GameManager not found in scene.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !isInvincible)
        {
            StartInvincibility();
            other.gameObject.SetActive(false);

            // 하트 UI 갱신
            if (heart == 3 && heart3) heart3.sprite = binHeart;
            else if (heart == 2 && heart2) heart2.sprite = binHeart;
            else if (heart == 1 && heart1) heart1.sprite = binHeart;

            heart--;

            if (damageOverlay) damageOverlay.ShowDamageEffect();

            if (heart <= 0)
            {
                // ✅ stage 쓰지 말고 GameManager로 게임오버 처리
                if (gm) gm.GameOver();
                else Debug.LogError("GameOver skipped: GameManager missing.");
            }
        }
    }

    void StartInvincibility()
    {
        if (invincibilityCoroutine != null)
            StopCoroutine(invincibilityCoroutine);

        invincibilityCoroutine = StartCoroutine(InvincibilityRoutine());
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        float t = 0f;

        while (t < invincibilityDuration)
        {
            foreach (var r in renderers) r.enabled = !r.enabled;
            yield return new WaitForSeconds(blinkInterval);
            t += blinkInterval;
        }

        foreach (var r in renderers) r.enabled = true;
        isInvincible = false;
    }
}