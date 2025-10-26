using System.Collections;
using UnityEngine;

public class PlayerInvincible : MonoBehaviour
{
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 2f;
    public float blinkInterval = 0.2f;
    bool isInvincible = false;

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

    GameManager gm;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            Debug.LogError("[PlayerInvincible] Renderer missing on player!");

        gm = FindObjectOfType<GameManager>();
        if (!gm) Debug.LogError("[PlayerInvincible] GameManager not found in scene.");

        if (!damageOverlay)
            damageOverlay = GetComponent<DamageOverlay>();
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

            if (damageOverlay)
                damageOverlay.ShowDamageEffect();

            if (heart <= 0)
            {
                if (gm)
                    gm.GameOver(); // ✅ GameManager 통해 게임오버
                else
                    Debug.LogError("[PlayerInvincible] GameManager missing, cannot GameOver.");
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
        float timer = 0f;

        while (timer < invincibilityDuration)
        {
            foreach (var r in renderers)
                r.enabled = !r.enabled;

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        foreach (var r in renderers)
            r.enabled = true;

        isInvincible = false;
    }
}