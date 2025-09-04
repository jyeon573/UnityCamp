using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bullet collided with Player!");
            Destroy(gameObject); // Player와 충돌 시 삭제
        }
        // 벽이나 다른 객체와 충돌은 무시
    }
}
