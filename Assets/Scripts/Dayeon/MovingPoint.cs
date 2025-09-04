using UnityEngine;

public class MovingPoint : MonoBehaviour
{
    public Vector3 moveDirection; // 이동 방향
    public float moveRange = 10f; // 이동 범위
    public float moveSpeedMin = 2f; // 최소 이동 속도
    public float moveSpeedMax = 5f; // 최대 이동 속도

    private float moveSpeed; // 실제 이동 속도
    private Vector3 startPos; // 초기 위치
    private float randomOffset; // 랜덤 시작 지점 (시간 기준)

    void Start()
    {
        startPos = transform.position; // 시작 위치 저장
        moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax); // 랜덤 속도 설정
        randomOffset = Random.Range(0f, 2f * Mathf.PI); // 랜덤 오프셋 설정
    }

    void Update()
    {
        // 랜덤 오프셋으로 사인파 시작 지점 변경
        float offset = Mathf.Sin(Time.time * moveSpeed + randomOffset) * moveRange;
        transform.position = startPos + moveDirection * offset;
    }
}
