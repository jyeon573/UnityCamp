using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float invincibilityDuration = 2f; // 무적 상태 지속 시간
    public float blinkInterval = 0.2f;       // 깜빡이는 간격
    public bool isInvincible = false;      // 현재 무적 상태인지
    private Renderer[] renderers;           // 모든 Renderer를 저장
    private Coroutine invincibilityCoroutine;

    public DamageOverlay damageOverlay;
    public UnityEngine.UI.Image heart3;
    public UnityEngine.UI.Image heart2;
    public UnityEngine.UI.Image heart1;
    public Sprite binHeart;
    public GameManager gameManager;
    public int heart;

    void Start()
    {
        // 모든 Renderer를 하위 오브젝트에서 검색
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("No Renderer found on player or its children!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 총알 태그가 있는 오브젝트와 충돌했을 때
        if (other.CompareTag("Bullet") && !isInvincible)
        {
            StartInvincibility();
            other.gameObject.SetActive(false);
            if(heart==3){
                heart3.sprite= binHeart;
            }
            else if(heart==2){
                heart2.sprite= binHeart;
            }
            else if(heart==1){
                heart1.sprite= binHeart;
            }
            heart--;
            GetComponent<DamageOverlay>().ShowDamageEffect();

            if (heart <= 0){
                SceneManager.LoadScene(gameManager.stage+2);
            }
        }
    }

    void StartInvincibility()
    {
        if (invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
        }
        invincibilityCoroutine = StartCoroutine(InvincibilityRoutine());
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        float timer = 0f;

        while (timer < invincibilityDuration)
        {
            // Renderer들을 순회하며 깜빡임 처리
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = !renderer.enabled;
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // 모든 Renderer를 복원
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        isInvincible = false;
    }
}
