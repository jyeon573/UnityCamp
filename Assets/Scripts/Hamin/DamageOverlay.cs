using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class DamageOverlay : MonoBehaviour
{
    public Image damageImage;          // UI Image (DamageOverlay)
    public float maxAlpha = 0.5f;      // 최대 Alpha 값
    public float fadeDuration = 1f;    // 페이드 아웃 시간

    private Color imageColor;          // Image의 현재 색상

    void Start()
    {
        if (damageImage != null)
        {
            // 초기 Color 값을 가져옴
            imageColor = damageImage.color;

            // 시작 시 Alpha를 0으로 설정
            imageColor.a = 0f;
            damageImage.color = imageColor;
        }
        else
        {
            Debug.LogError("DamageOverlay Image is not assigned!");
        }
    }

    public void ShowDamageEffect()
    {
        if (damageImage != null)
        {
            // Alpha 값을 최대값으로 설정
            imageColor.a = maxAlpha;
            damageImage.color = imageColor;

            // 페이드 아웃 시작
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Alpha 값을 점진적으로 감소
            imageColor.a = Mathf.Lerp(maxAlpha, 0f, elapsedTime / fadeDuration);
            damageImage.color = imageColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Alpha 값을 완전히 0으로 설정
        imageColor.a = 0f;
        damageImage.color = imageColor;
    }
}
