using UnityEngine;

public class Projectile3 : MonoBehaviour
{
    private Vector3 direction; // 투사체 방향
    public float speed = 10f; // 투사체 속도
    public GameObject explosionEffectPrefab;

    // 방향 설정 메서드
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    // 투사체와 다른 객체와의 충돌 처리
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 적절한 태그를 가지고 있는지 확인
        if (other.CompareTag("Obstacle") || other.CompareTag("Player"))
        {
            // 충돌 이펙트 생성
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // 투사체 파괴
            Destroy(gameObject);
        }
    }
}
