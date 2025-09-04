using UnityEngine;
using System.Collections;

public class PointManager : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform[] movingPoints; // 총구 배열
    public float fireInterval = 0.5f; // 발사 간격
    public float bulletSpeed = 30f; // 총알 속도
    public float bulletLifeTime = 0.7f; // 총알 생존 시간
    public Color gunDefaultColor = Color.red; // 총구 기본 색상
    public Color gunBlinkColor = Color.yellow; // 총구 깜박임 색상
    public float blinkDuration = 1f; // 총구 깜박임 지속 시간
    public int blinkFrequency = 3; // 깜박임 횟수

    private Renderer[] gunRenderers; // 총구 렌더러 배열
    private int[] currentGunIndices; // 현재 선택된 총구 인덱스 배열

    void Start()
    {
        // 총구 렌더러 초기화
        gunRenderers = new Renderer[movingPoints.Length];
        for (int i = 0; i < movingPoints.Length; i++)
        {
            gunRenderers[i] = movingPoints[i].GetComponent<Renderer>();
            if (gunRenderers[i] != null)
            {
                gunRenderers[i].material.color = gunDefaultColor; // 기본 색상으로 초기화
            }
        }

        currentGunIndices = new int[3]; // 한 번에 사용할 총구 3개
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1f); // 1초 지연
        StartCoroutine(FireBulletsFromThreeRandomGuns());
    }

    IEnumerator FireBulletsFromThreeRandomGuns()
    {
        while (true)
        {
            // 랜덤으로 3개의 총구 선택
            for (int i = 0; i < 3; i++)
            {
                currentGunIndices[i] = Random.Range(0, movingPoints.Length);
            }

            // 선택된 총구에서 발사 준비
            foreach (int gunIndex in currentGunIndices)
            {
                Renderer renderer = gunRenderers[gunIndex];
                Transform gun = movingPoints[gunIndex];

                // 총구를 깜박이고 나서 총알 발사
                yield return StartCoroutine(BlinkGun(renderer));
                FireBullet(gun);
            }

            // 발사 간격 대기
            yield return new WaitForSeconds(fireInterval);
        }
    }

    void FireBullet(Transform gun)
    {
        GameObject bullet = Instantiate(bulletPrefab, gun.position, gun.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = gun.forward * bulletSpeed;
        }
        Destroy(bullet, bulletLifeTime); // 일정 시간 후 총알 제거
    }

    IEnumerator BlinkGun(Renderer renderer)
    {
        float blinkTime = blinkDuration / blinkFrequency;
        for (int i = 0; i < blinkFrequency; i++)
        {
            renderer.material.color = gunBlinkColor; // 깜박임 색상으로 변경
            yield return new WaitForSeconds(blinkTime / 3);
            renderer.material.color = gunDefaultColor; // 기본 색상으로 복원
            yield return new WaitForSeconds(blinkTime / 3);
        }
    }
}
