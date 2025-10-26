using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    public float speed = 3f; // 이동 속도

    private GameManager gm;

    void Start()
    {
        // 시작할 때 GameManager 찾아서 캐싱
        gm = FindObjectOfType<GameManager>();
        if (!gm)
        {
            Debug.LogError("[PlayerMovement] ❌ GameManager not found in scene");
        }
    }

    void Update()
    {
        // 혹시 씬 전환 등으로 gm이 사라졌으면 다시 찾아줌
        if (!gm)
        {
            gm = FindObjectOfType<GameManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coin"))
        {
            Debug.Log("[PlayerMovement] ✅ Coin hit");

            // GameManager 점수 증가
            if (gm)
            {
                gm.CountUp(1);
                Debug.Log("[PlayerMovement] ✅ CountUp(1)");
            }
            else
            {
                Debug.LogWarning("[PlayerMovement] ⚠️ GameManager still missing");
            }

            // 코인 위치 재배치
            float x = Random.Range(-8.5f, 8.5f);
            float z = Random.Range(-8.5f, 8.5f);
            other.transform.position = new Vector3(x, -1.5f, z);
        }
    }
}